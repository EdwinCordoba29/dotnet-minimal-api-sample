using System.ComponentModel.DataAnnotations;

namespace dotnet_minimal_api_sample.DTO
{
    public class UserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
