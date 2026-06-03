using dotnet_minimal_api_sample.Data;
using dotnet_minimal_api_sample.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace dotnet_minimal_api_sample.Services
{
    public class ServiceSale : IServiceSales
    {
        private string ConnectionString;
        private readonly ILogger<ServiceSale> log;
        public ServiceSale(ConnectionFactory connectionString, ILogger<ServiceSale> log)
        {
            ConnectionString = connectionString.SQLConnectionString;
            this.log = log;
        }

        private SqlConnection connection()
        {
            return new SqlConnection(ConnectionString);
        }
        public async Task CreateSale(Sale sale, string user)
        {
            using (SqlConnection sqlConnection = connection())
            {
                await sqlConnection.OpenAsync();
                using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        int currentSaleId = 0;
                        DateTime SaleDateTime = DateTime.Now;
                        using (SqlCommand command = sqlConnection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "dbo.CreateSale";
                            foreach (var Product in sale.Products)
                            {
                                command.Parameters.Clear();
                                command.Parameters.Add("@CustomerDocumentNumber", SqlDbType.NVarChar, 30).Value = sale.CustomerDocumentNumber;
                                command.Parameters.Add("@ProductCode", SqlDbType.NVarChar, 50).Value = Product.ProductCode;
                                command.Parameters.Add("@SaleUserId", SqlDbType.Int).Value = int.Parse(user);
                                command.Parameters.Add("@SaleDateTime", SqlDbType.DateTime2).Value = SaleDateTime;
                                command.Parameters.Add("@Observations", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(sale.Observations) ? DBNull.Value : sale.Observations;
                                command.Parameters.Add("@Quantity", SqlDbType.Int).Value = Product.Quantity;
                                command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = (Product.Discount == null || Product.Discount == 0) ? DBNull.Value : Product.Discount;
                                command.Parameters.Add("@SaleId", SqlDbType.Int).Value = currentSaleId == 0 ? DBNull.Value : currentSaleId;

                                var result = await command.ExecuteScalarAsync();
                                currentSaleId = Convert.ToInt32(result);
                            }
                        }
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        if (transaction != null)
                        {
                            await transaction.RollbackAsync();
                        }
                        log.LogError("ERROR: " + ex.ToString());
                        throw new Exception("Se produjo un error al crear la venta. ");
                    }
                }
            }
        }

        public async Task<IEnumerable<SaleDetailsDTO>> GetSales() 
            => await GetSalesInternal(null, null);

        public async Task<IEnumerable<SaleDetailsDTO>> GetSalesByCustomer(string documentNumber)
            => await GetSalesInternal(documentNumber, null);

        public async Task<IEnumerable<SaleDetailsDTO>> GetSaleByInvoice(string invoiceNumber)
            => await GetSalesInternal(null, invoiceNumber);

        private async Task<IEnumerable<SaleDetailsDTO>> GetSalesInternal(string customerDoc, string invoiceNum)
        {
            var salesList = new List<SaleDetailsDTO>();

            using (SqlConnection sqlConnection = connection())
            {
                await sqlConnection.OpenAsync();
                try
                {
                    using (SqlCommand command = sqlConnection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.ListSales";
                        command.Parameters.Add("@CustomerDocumentNumber", SqlDbType.NVarChar, 30).Value = string.IsNullOrWhiteSpace(customerDoc) ? DBNull.Value : customerDoc;
                        command.Parameters.Add("@InvoiceNumber", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(invoiceNum) ? DBNull.Value : invoiceNum;

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int saleId = reader.GetInt32(reader.GetOrdinal("SaleId"));

                                // Buscamos si ya agregamos esta factura a la lista para no repetirla
                                var sale = salesList.FirstOrDefault(s => s.SaleId == saleId);

                                if (sale == null)
                                {
                                    sale = new SaleDetailsDTO
                                    {
                                        SaleId = saleId,
                                        InvoiceNumber = reader.GetString(reader.GetOrdinal("InvoiceNumber")),
                                        SaleDateTime = reader.GetDateTime(reader.GetOrdinal("SaleDateTime")),
                                        CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                        CustomerDocument = reader.GetString(reader.GetOrdinal("CustomerDocument")),
                                        Observations = reader.IsDBNull(reader.GetOrdinal("Observations")) ? null : reader.GetString(reader.GetOrdinal("Observations")),
                                        State = reader.GetBoolean(reader.GetOrdinal("State")),
                                        Products = new List<SaleDetailsItemDTO>()
                                    };
                                    salesList.Add(sale);
                                }

                                // Agregamos la línea del producto a la factura correspondiente
                                sale.Products.Add(new SaleDetailsItemDTO
                                {
                                    ProductCode = reader.GetString(reader.GetOrdinal("ProductCode")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                                    Discount = reader.IsDBNull(reader.GetOrdinal("Discount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Discount"))
                                });
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al consultar ventas.");
                }
                
            }
            return salesList;
        }

        public async Task CancelSale(string invoiceNumber, string user)
        {
            using (SqlConnection sqlConnection = connection())
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = sqlConnection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.CancelSale";
                        command.Parameters.Add("@InvoiceNumber", SqlDbType.NVarChar, 50).Value = invoiceNumber;
                        command.Parameters.Add("@CancellationDate", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@UserId", SqlDbType.Int).Value = int.Parse(user);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (SqlException ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al cancelar la venta.");
                }
            }
        }
        
    }
}
