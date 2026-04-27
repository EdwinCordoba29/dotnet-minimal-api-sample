using System.ComponentModel.DataAnnotations;

namespace dotnet_minimal_api_sample.DTO
{
    public class ProductDTO
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
    }
}
