using dotnet_minimal_api_sample.Data;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Linq.Expressions;

namespace dotnet_minimal_api_sample.Services
{
    public class ServiceProduct : IServiceProducts
    {
        private string ConnectionString;

        public ServiceProduct(ConnectionFactory connectionString)
        {
            ConnectionString = connectionString.SQLConnectionString;
        }

        private SqlConnection connection()
        {
            return new SqlConnection(ConnectionString);
        }

        public async Task CreateProduct(Product product)
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
                        command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = product.Description;
                        command.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = product.Code;
                        command.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = product.Price });
                        command.Parameters.Add("@Stock", SqlDbType.Int).Value = product.Stock;
                        command.Parameters.Add("@State", SqlDbType.Bit).Value = 1;
                        command.Parameters.Add("@CreationDate", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@CreatedByUserId", SqlDbType.NVarChar, 50).Value = "123";
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Se produjo un error al crear el producto: " + ex.Message);
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
                                Description = reader.GetString("description"),
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
                    throw new Exception("Se produjo un error al listar los productos: " + ex.Message);
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
                                Description = reader.GetString("description"),
                                Code = reader.GetString("code"),
                                Price = reader.GetDecimal("Price"),
                                Stock = reader.GetInt32("Stock")
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Se produjo un error al listar el producto: " + ex.Message);
                }

                return product;
            }
        }

        public async Task UpdateProduct(Product product)
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
                        command.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = product.Description;
                        command.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = product.Code;
                        command.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = product.Price });
                        command.Parameters.Add("@Stock", SqlDbType.Int).Value = product.Stock;
                        command.Parameters.Add("@UpdateDate", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@UpdatedByUserId", SqlDbType.NVarChar, 50).Value = "456";

                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Se produjo un error al actualizar el producto." + ex.Message);
                }
            }
        }

        public async Task DeleteProduct(string Code)
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
                        command.Parameters.Add("@DeletedByUserId", SqlDbType.NVarChar, 50).Value = "789";
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Se produjo un error al eliminar el producto." + ex.Message);
                }
            }
        }
    }
}
