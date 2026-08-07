using Npgsql;
using TestManagement.API.Infrastructure.Configuration;
using TestManagement.API.Infrastructure.IO;

namespace TestManagement.API.Infrastructure.Database
{
    /// <summary>
    /// Factory responsible for constructing database connection strings.
    /// </summary>
    public class DBConnectionFactory : IDBConnectionFactory
    {
        /// <summary>
        /// Configuration utility used to read environment or appsettings values.
        /// </summary>
        private readonly IConfigUtility _configUtility;

        /// <summary>
        /// File reader used to read secret values (for example, password files).
        /// </summary>
        private readonly IFileReader _fileReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBConnectionFactory"/> class.
        /// </summary>
        /// <param name="configUtility">The configuration utility instance.</param>
        /// <param name="fileReader">The file reader instance.</param>
        public DBConnectionFactory(
            IConfigUtility configUtility,
            IFileReader fileReader
            )
        {
            _configUtility = configUtility;
            _fileReader = fileReader;
        }

        /// <summary>
        /// Creates a PostgreSQL connection string using configuration values and secret files.
        /// </summary>
        /// <returns>A valid PostgreSQL connection string.</returns>
        /// <exception cref="System.FormatException">Thrown when DB_PORT cannot be parsed as an integer.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when a required configuration value is missing.</exception>
        public string CreatePostgresConnectionString()
        {
            string host = _configUtility.GetValue("DB_HOST");
            string port = _configUtility.GetValue("DB_PORT");
            string database = _configUtility.GetValue("DB_NAME");
            string user = _configUtility.GetValue("DB_USER");
            string passFilePath = _configUtility.GetValue("DB_PASSWORD_FILE");

            string password = _fileReader.ReadAllText(passFilePath);

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
