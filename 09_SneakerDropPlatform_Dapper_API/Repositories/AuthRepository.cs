using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SneakerDropPlatform.Repositories
{
    public class AuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<bool> ValidateAdminAsync(string username, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(1) FROM Admins WHERE Username = @Username AND Password = @Password";
                int count = await connection.ExecuteScalarAsync<int>(sql, new { Username = username, Password = password });
                return count > 0;
            }
        }
    }
}
