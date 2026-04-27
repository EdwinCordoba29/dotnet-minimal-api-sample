using dotnet_minimal_api_sample.Data;
using System.Collections;

namespace dotnet_minimal_api_sample.Services
{
    public interface IServiceProducts
    {
        Task CreateProduct(Product product);
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(string code);
        Task UpdateProduct(Product product);
        Task DeleteProduct(string Code);

    }
}
