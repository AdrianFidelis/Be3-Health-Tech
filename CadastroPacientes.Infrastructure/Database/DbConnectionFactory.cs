using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CadastroPacientes.Infrastructure.Database
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() =>
            new SqlConnection(_connectionString);
    }
}
