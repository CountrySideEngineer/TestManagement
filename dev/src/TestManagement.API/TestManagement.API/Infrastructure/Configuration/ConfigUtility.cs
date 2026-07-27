namespace TestManagement.API.Infrastructure.Configuration
{
    /// <summary>
    /// Utility methods for reading configuration and environment values.
    /// </summary>
    public class ConfigUtility : IConfigUtility
    {
        private readonly IConfiguration _config;

        public ConfigUtility(
            IConfiguration config
            )
        {
            _config = config;
        }

        public string GetValue(string key, bool required = true)
        {
            string? value = _config[key];

            if (required && string.IsNullOrEmpty(value))
                throw new InvalidOperationException($"{key} is not set in environment variables or appsettings.");

            return value!;
        }
    }
}
