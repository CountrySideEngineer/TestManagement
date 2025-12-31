using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManagement.Analyze.APP.Infrastructure.Configuration
{
    internal class ConfigUtility
    {
        public static string GetValue(IConfiguration config, string key, bool required = true)
        {
            string? value = config[key];

            if (required && string.IsNullOrEmpty(value))
                throw new InvalidOperationException($"{key} is not set in environment variables or appsettings.");

            return value!;
        }
    }
}
