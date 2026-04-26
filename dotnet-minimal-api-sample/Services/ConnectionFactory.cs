using Microsoft.AspNetCore.Hosting.Server;

namespace dotnet_minimal_api_sample.Services
{
    public class ConnectionFactory
    {
        private string ConnectionString;

        public string SQLConnectionString { get => ConnectionString; }
        public ConnectionFactory(String connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
