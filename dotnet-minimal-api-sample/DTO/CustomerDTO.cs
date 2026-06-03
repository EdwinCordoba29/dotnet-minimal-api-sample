using System.ComponentModel.DataAnnotations;

namespace dotnet_minimal_api_sample.DTO
{
    public class CustomerDTO
    {
        [Required]
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        [Required]
        public string DocumentType { get; set; }
        [Required]
        public string DocumentNumber { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
    }
}
