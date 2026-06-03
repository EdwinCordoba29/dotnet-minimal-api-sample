using dotnet_minimal_api_sample.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace dotnet_minimal_api_sample.Services
{
    public class ServiceCustomer : IServiceCustomers
    {
        private string ConnectionString;
        private readonly ILogger<ServiceCustomer> log;

        public ServiceCustomer(ConnectionFactory connectionString, ILogger<ServiceCustomer> log)
        {
            ConnectionString = connectionString.SQLConnectionString;
            this.log = log;
        }

        private SqlConnection connection()
        {
            return new SqlConnection(ConnectionString);
        }

        public async Task CreateCustomer(Customer customer, string user)
        {
            using (SqlConnection sqlConnection = connection())
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = sqlConnection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.CreateCustomer";
                        command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 100).Value = customer.FirstName;
                        command.Parameters.Add("@MiddleName", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(customer.MiddleName) ? DBNull.Value : customer.MiddleName;
                        command.Parameters.Add("@LastName", SqlDbType.NVarChar, 100).Value = customer.LastName;
                        command.Parameters.Add("@SecondLastName", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(customer.SecondLastName) ? DBNull.Value : customer.SecondLastName;
                        command.Parameters.Add("@DocumentType", SqlDbType.NVarChar, 20).Value = customer.DocumentType;
                        command.Parameters.Add("@DocumentNumber", SqlDbType.NVarChar, 30).Value = customer.DocumentNumber;
                        command.Parameters.Add("@Email", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(customer.Email) ? DBNull.Value : customer.Email;
                        command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = string.IsNullOrWhiteSpace(customer.Phone) ? DBNull.Value : customer.Phone;
                        command.Parameters.Add("@City", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(customer.City) ? DBNull.Value : customer.City;
                        command.Parameters.Add("@Address", SqlDbType.NVarChar, 255).Value = string.IsNullOrWhiteSpace(customer.Address) ? DBNull.Value : customer.Address;
                        command.Parameters.Add("@State", SqlDbType.Bit).Value = 1;
                        command.Parameters.Add("@CreationDate", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@CreatedByUserId", SqlDbType.Int).Value = int.Parse(user);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al crear el cliente.");
                }
            }
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            using (SqlConnection sqlConnection = connection())
            {
                List<Customer> customers = new List<Customer>();
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("dbo.ListCustomers", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                FirstName = reader.GetString("FirstName"),
                                MiddleName = reader.IsDBNull("MiddleName") ? null : reader.GetString("MiddleName"),
                                LastName = reader.GetString("LastName"),
                                SecondLastName = reader.IsDBNull("SecondLastName") ? null : reader.GetString("SecondLastName"),
                                DocumentType = reader.GetString("DocumentType"),
                                DocumentNumber = reader.GetString("DocumentNumber"),
                                Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                                Phone = reader.IsDBNull("Phone") ? null : reader.GetString("Phone"),
                                City = reader.IsDBNull("City") ? null : reader.GetString("City"),
                                Address = reader.IsDBNull("Address") ? null : reader.GetString("Address")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al listar los clientes.");
                }
                return customers;
            }
        }

        public async Task<Customer> GetCustomer(string documentNumber)
        {
            using (SqlConnection sqlConnection = connection())
            {
                Customer customer = null;
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("dbo.ListCustomers", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@DocumentNumber", SqlDbType.NVarChar, 30).Value = documentNumber;
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.Read())
                        {
                            customer = new Customer
                            {
                                FirstName = reader.GetString("FirstName"),
                                MiddleName = reader.IsDBNull("MiddleName") ? null : reader.GetString("MiddleName"),
                                LastName = reader.GetString("LastName"),
                                SecondLastName = reader.IsDBNull("SecondLastName") ? null : reader.GetString("SecondLastName"),
                                DocumentType = reader.GetString("DocumentType"),
                                DocumentNumber = reader.GetString("DocumentNumber"),
                                Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                                Phone = reader.IsDBNull("Phone") ? null : reader.GetString("Phone"),
                                City = reader.IsDBNull("City") ? null : reader.GetString("City"),
                                Address = reader.IsDBNull("Address") ? null : reader.GetString("Address")
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al buscar el cliente.");
                }
                return customer;
            }
        }

        public async Task UpdateCustomer(Customer customer, string user)
        {
            using (SqlConnection sqlConnection = connection())
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = sqlConnection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.UpdateCustomer";
                        command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 100).Value = customer.FirstName;
                        command.Parameters.Add("@MiddleName", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(customer.MiddleName) ? DBNull.Value : customer.MiddleName;
                        command.Parameters.Add("@LastName", SqlDbType.NVarChar, 100).Value = customer.LastName;
                        command.Parameters.Add("@SecondLastName", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(customer.SecondLastName) ? DBNull.Value : customer.SecondLastName;
                        command.Parameters.Add("@DocumentType", SqlDbType.NVarChar, 20).Value = customer.DocumentType;
                        command.Parameters.Add("@DocumentNumber", SqlDbType.NVarChar, 30).Value = customer.DocumentNumber;
                        command.Parameters.Add("@Email", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(customer.Email) ? DBNull.Value : customer.Email;
                        command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = string.IsNullOrWhiteSpace(customer.Phone) ? DBNull.Value : customer.Phone;
                        command.Parameters.Add("@City", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(customer.City) ? DBNull.Value : customer.City;
                        command.Parameters.Add("@Address", SqlDbType.NVarChar, 255).Value = string.IsNullOrWhiteSpace(customer.Address) ? DBNull.Value : customer.Address;
                        command.Parameters.Add("@UpdateDate", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@UpdatedByUserId", SqlDbType.Int).Value = int.Parse(user);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al actualizar el cliente.");
                }
            }
        }

        public async Task DeleteCustomer(string documentNumber, string user)
        {
            using (SqlConnection sqlConnection = connection())
            {
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = sqlConnection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.DeleteCustomer";
                        command.Parameters.Add("@DocumentNumber", SqlDbType.NVarChar, 30).Value = documentNumber;
                        command.Parameters.Add("@DeletedDate", SqlDbType.DateTime2).Value = DateTime.Now;
                        command.Parameters.Add("@DeletedByUserId", SqlDbType.Int).Value = int.Parse(user);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Se produjo un error al eliminar el cliente.");
                }
            }
        }
    }
}
