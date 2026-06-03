using dotnet_minimal_api_sample.Data;

namespace dotnet_minimal_api_sample.Services
{
    public interface IServiceProducts
    {
        Task CreateProduct(Product product, string user);
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(string code);
        Task UpdateProduct(Product product, string user);
        Task DeleteProduct(string Code, string user);

    }
}
