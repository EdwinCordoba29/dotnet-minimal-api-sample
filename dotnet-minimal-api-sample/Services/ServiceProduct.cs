using dotnet_minimal_api_sample.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace dotnet_minimal_api_sample.Services
{
    public class ServiceProduct : IServiceProducts
    {
        private string ConnectionString;
        private readonly ILogger<ServiceProduct> log;
        public ServiceProduct(ConnectionFactory connectionString, ILogger<ServiceProduct> log)
        {
            ConnectionString = connectionString.SQLConnectionString;
            this.log = log;
        }

        private SqlConnection connection()
        {
            return new SqlConnection(ConnectionString);
        }

        public async Task CreateProduct(Product product, string user)
        {
            using (SqlConnection sqlConnection = connection())
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = sqlConnection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.CreateProduct";
                        command.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = product.Name;
                        command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = string.IsNullOrWhiteSpace(product.Description) ? DBNull.Value : product.Description;
                        command.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = product.Code;
                        command.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = product.Price });
                        command.Parameters.Add("@Stock", SqlDbType.Int).Value = product.Stock;
                        command.Parameters.Add("@State", SqlDbType.Bit).Value = 1;
                        command.Parameters.Add("@CreationDate", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@CreatedByUserId", SqlDbType.Int).Value = int.Parse(user);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al crear el producto. ");
                }
            }
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            using (SqlConnection sqlConnection = connection())
            {
                List<Product> products = new List<Product>();
                Product product = null;
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("dbo.ListProducts", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            product = new Product
                            {
                                Name = reader.GetString("name"),
                                Description = reader.IsDBNull("description") ? null : reader.GetString("description"),
                                Code = reader.GetString("code"),
                                Price = reader.GetDecimal("Price"),
                                Stock = reader.GetInt32("Stock")
                            };
                            products.Add(product);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al listar los productos. ");
                }
                return products;
            }
        }

        public async Task<Product> GetProduct(string code)
        {
            using (SqlConnection sqlConnection = connection())
            {
                Product product = null;
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("dbo.ListProducts", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = code;
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            product = new Product
                            {
                                Name = reader.GetString("name"),
                                Description = reader.IsDBNull("description") ? null : reader.GetString("description"),
                                Code = reader.GetString("code"),
                                Price = reader.GetDecimal("Price"),
                                Stock = reader.GetInt32("Stock")
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al listar el producto. ");
                }

                return product;
            }
        }

        public async Task UpdateProduct(Product product, string user)
        {
            using (SqlConnection sqlConnection = connection())
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = sqlConnection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.UpdateProduct";
                        command.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = product.Name;
                        command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = string.IsNullOrWhiteSpace(product.Description) ? DBNull.Value : product.Description;
                        command.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = product.Code;
                        command.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = product.Price });
                        command.Parameters.Add("@Stock", SqlDbType.Int).Value = product.Stock;
                        command.Parameters.Add("@UpdateDate", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@UpdatedByUserId", SqlDbType.Int).Value = int.Parse(user);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al actualizar el producto.");
                }
            }
        }

        public async Task DeleteProduct(string Code, string user)
        {
            using (SqlConnection sqlConnection = connection())
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = sqlConnection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.DeleteProduct";
                        command.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = Code;
                        command.Parameters.Add("@DeletedDate", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@DeletedByUserId", SqlDbType.Int).Value = int.Parse(user);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al eliminar el producto.");
                }
            }
        }
    }
}
