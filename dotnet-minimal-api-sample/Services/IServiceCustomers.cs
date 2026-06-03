using dotnet_minimal_api_sample.Data;

namespace dotnet_minimal_api_sample.Services
{
    public interface IServiceCustomers
    {
        Task CreateCustomer(Customer customer, string user);
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Customer> GetCustomer(string documentNumber);
        Task UpdateCustomer(Customer customer, string user);
        Task DeleteCustomer(string documentNumber, string user);
    }
}
