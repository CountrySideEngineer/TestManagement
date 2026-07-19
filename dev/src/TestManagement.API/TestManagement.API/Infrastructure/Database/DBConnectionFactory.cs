using Npgsql;
using TestManagement.API.Infrastructure.Configuration;

namespace TestManagement.API.Infrastructure.Database
{
    /// <summary>
    /// Factory responsible for constructing database connection strings.
    /// </summary>
    public class DBConnectionFactory
    {
        /// <summary>
        /// Builds a PostgreSQL connection string from configuration values.
        /// </summary>
        /// <param name="config">The configuration instance used to read database settings.</param>
        /// <returns>A fully populated PostgreSQL connection string.</returns>
        /// <remarks>
        /// This method expects configuration values for DB_HOST, DB_PORT, DB_NAME, DB_USER and DB_PASSWORD_FILE.
        /// The database password is read from the file path specified by DB_PASSWORD_FILE.
        /// </remarks>
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
            };

            return builder.ConnectionString;
        }
    }
}
