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
        private readonly ILogger<ServiceAuth> log;

        public ServiceAuth(ConnectionFactory connectionString, IConfiguration configuration, ILogger<ServiceAuth> log)
        {
            ConnectionString = connectionString.SQLConnectionString;
            Configuration = configuration;
            this.log = log;
        }

        private SqlConnection connection()
        {
            return new SqlConnection(ConnectionString);
        }

        public async Task<string> Login(UserDTO userDTO)
        {
            User user = null;
            string token = String.Empty;
            user = await GetUserByUsernameAsync(userDTO.UserName);
            if (user is null || !BCrypt.Net.BCrypt.Verify(userDTO.Password, user.Password))
            {
                throw new Exception("Credenciales no válidas.");
            }
            else
            {
                token = GenerateTokenJWT(user);
            }
            return token;
        }

        private async Task<User> GetUserByUsernameAsync(string userName)
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
                        command.Parameters.Add("@UserName",SqlDbType.NVarChar,200).Value = userName;
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = (int) reader["Id"],
                                UserName = reader["UserName"].ToString(),
                                Password = reader["Password"].ToString(),
                                Email = reader["Email"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.LogError("ERROR: " + ex.ToString());
                    throw new Exception("Error al realizar el login.");
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
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("preferred_username", user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
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
