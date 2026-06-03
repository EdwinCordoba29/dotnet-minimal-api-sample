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

        public static CustomerDTO ConvertDTO(this Customer customer)
        {
            if (customer != null)
            {
                return new CustomerDTO
                {
                    FirstName = customer.FirstName,
                    MiddleName = customer.MiddleName,
                    LastName = customer.LastName,
                    SecondLastName = customer.SecondLastName,
                    DocumentType = customer.DocumentType,
                    DocumentNumber = customer.DocumentNumber,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    City = customer.City,
                    Address = customer.Address
                };
            }

            return null;
        }

    }
}
