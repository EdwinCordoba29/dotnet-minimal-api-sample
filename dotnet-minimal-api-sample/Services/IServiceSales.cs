using dotnet_minimal_api_sample.Data;
using dotnet_minimal_api_sample.DTO;

namespace dotnet_minimal_api_sample.Services
{
    public interface IServiceSales
    {
        Task CreateSale(Sale sale, string user);
        Task<IEnumerable<SaleDetailsDTO>> GetSales();
        Task<IEnumerable<SaleDetailsDTO>> GetSalesByCustomer(string documentNumber);
        Task<IEnumerable<SaleDetailsDTO>> GetSaleByInvoice(string invoiceNumber);
        Task CancelSale(string invoiceNumber, string user);
    }
}
