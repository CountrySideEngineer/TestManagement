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
        private readonly IConfigUtility _configUtility;

        private readonly IFileReader _fileReader;

        public DBConnectionFactory(
            IConfigUtility configUtility,
            IFileReader fileReader
            )
        {
            _configUtility = configUtility;
            _fileReader = fileReader;
        }

        public string CreatePostgresConnectionString()
        {
            string host = _configUtility.GetValue("DB_HOST");
            string port = _configUtility.GetValue("DB_PORT");
            string database = _configUtility.GetValue("DB_NAME");
            string user = _configUtility.GetValue("DB_USER");
            string passFilePath = _configUtility.GetValue( "DB_PASSWORD_FILE");

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
