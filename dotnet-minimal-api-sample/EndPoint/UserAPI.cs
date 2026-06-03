using dotnet_minimal_api_sample.DTO;
using dotnet_minimal_api_sample.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_minimal_api_sample.EndPoint
{
    public static class UserAPI
    {
        public static void ConfigureUserEndPoint(this WebApplication app) 
        {
            app.MapPost("/api/login", Login).AllowAnonymous();
        }

        private async static Task<IResult> Login([FromBody] UserDTO userDTO, IServiceAuth serviceAuth) 
        {
            string token = await serviceAuth.Login(userDTO);
            if (token == String.Empty)
            {
                return Results.NotFound("Usuario no existe.");
            }
            else
            {
                return Results.Ok(token);
            }
        }
    }
}
