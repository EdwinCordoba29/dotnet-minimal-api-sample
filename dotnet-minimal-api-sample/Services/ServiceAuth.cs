using dotnet_minimal_api_sample.Data;
using dotnet_minimal_api_sample.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dotnet_minimal_api_sample.Services
{
    public class ServiceAuth : IServiceAuth
    {
        private string ConnectionString;
        private readonly IConfiguration Configuration;

        public ServiceAuth(ConnectionFactory connectionString, IConfiguration configuration)
        {
            ConnectionString = connectionString.SQLConnectionString;
            Configuration = configuration;
        }

        private SqlConnection connection()
        {
            return new SqlConnection(ConnectionString);
        }

        public async Task<string> Login(UserDTO userDTO)
        {
            User user = null;
            string token = String.Empty;
            user = await AuthenticateUserAsync(userDTO);
            if (user is null)
            {
                throw new Exception("Credenciales no válidas.");
            }
            else
            {
                token = GenerateTokenJWT(user);
            }
            return token;
        }

        private async Task<User> AuthenticateUserAsync(UserDTO userDTO)
        {
            using (SqlConnection sqlConnection = connection())
            {
                User user = null;
                try
                {
                    await sqlConnection.OpenAsync();
                    using (SqlCommand command = sqlConnection.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.ListUser";
                        command.Parameters.Add("@UserName",SqlDbType.NVarChar,200).Value = userDTO.UserName;
                        command.Parameters.Add("@Password",SqlDbType.NVarChar,200).Value = userDTO.Password;
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserName = reader["UserName"].ToString(),
                                EMail = reader["EMail"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al realizar el login: " + ex.Message);
                }
                return user; 
            }
        }

        private string GenerateTokenJWT(User user)
        {
            //Cabecera
            var _symmetricSecutiryKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration["JWT:ClaveSecreta"])
                );
            var _sinningCredentials = new SigningCredentials(
                _symmetricSecutiryKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_sinningCredentials);

            //Claims
            var Claims = new[]
            {
                new Claim("user", user.UserName),
                new Claim("email", user.EMail),
                new Claim(JwtRegisteredClaimNames.Email, user.EMail)
            };

            //Payload
            var _Payload = new JwtPayload(
                issuer: Configuration["JWT:Issuer"],
                audience: Configuration["JWT:Audience"],
                claims: Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1)
                );

            // Token
            var _Token = new JwtSecurityToken(
                _Header, _Payload
                );

            string Token = new JwtSecurityTokenHandler().WriteToken(_Token);

            return Token;
        }
    }
}
