using Npgsql;
using TestManagement.API.Infrastructure.Configuration;

namespace TestManagement.API.Infrastructure.Database
{
    public class DBConnectionFactory
    {
        public static string CreatePostgresConnectionString(IConfiguration config)
        {
            string host = ConfigUtility.GetValue(config, "DB_HOST");
            string port = ConfigUtility.GetValue(config, "DB_PORT");
            string database = ConfigUtility.GetValue(config, "DB_NAME");
            string user = ConfigUtility.GetValue(config, "DB_USER");
            string passFilePath = ConfigUtility.GetValue(config, "DB_PASSWORD_FILE");

            string password = File.ReadAllText(passFilePath).Trim();

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Port = int.Parse(port),
                Database = database,
                Username = user,
                Password = password,

                //SslMode = SslMode.Disable,
            };

            return builder.ConnectionString;
        }
    }
}
