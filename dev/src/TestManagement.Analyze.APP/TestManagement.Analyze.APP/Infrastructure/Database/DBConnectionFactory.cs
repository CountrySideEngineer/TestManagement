using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestManagement.Analyze.APP.Infrastructure.Configuration;

namespace TestManagement.Analyze.APP.Infrastructure.DBConnectionFactory
{
    internal class DBConnectionFactory
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
