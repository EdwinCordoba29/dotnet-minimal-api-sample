using dotnet_minimal_api_sample.Data;
using dotnet_minimal_api_sample.DTO;

namespace dotnet_minimal_api_sample
{
    public static class Utilities
    {
        public static ProductDTO ConvertDTO(this Product product)
        {
            if (product != null) 
            {
                return new ProductDTO
                { 
                    Name = product.Name,
                    Description = product.Description,
                    Code = product.Code,
                    Price = product.Price,
                    Stock = product.Stock
                };
            }

            return null;
        }
    }
}
