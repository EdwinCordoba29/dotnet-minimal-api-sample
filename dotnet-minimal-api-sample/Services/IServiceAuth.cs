using dotnet_minimal_api_sample.DTO;

namespace dotnet_minimal_api_sample.Services
{
    public interface IServiceAuth
    {
        Task<String> Login(UserDTO userDTO);
    }
}
