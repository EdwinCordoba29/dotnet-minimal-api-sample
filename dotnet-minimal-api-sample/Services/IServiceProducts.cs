using dotnet_minimal_api_sample.Data;

namespace dotnet_minimal_api_sample.Services
{
    public interface IServiceProducts
    {
        Task CreateProduct(Product product);
        Task<List<Product>> GetProducts();
        Task<Product> GetProduct(string code);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int Id);

    }
}
