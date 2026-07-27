namespace TestManagement.API.Infrastructure.Configuration
{
    /// <summary>
    /// Utility methods for reading configuration and environment values.
    /// </summary>
    public class ConfigUtility : IConfigUtility
    {
        /// <summary>
        /// Underlying configuration provider used to retrieve values by key.
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigUtility"/> class.
        /// </summary>
        /// <param name="config">The configuration provider to use.</param>
        public ConfigUtility(
            IConfiguration config
            )
        {
            _config = config;
        }

        /// <summary>
        /// Retrieves a configuration value by key.
        /// </summary>
        /// <param name="key">The configuration key to read.</param>
        /// <param name="required">If true, an exception is thrown when the value is missing or empty.</param>
        /// <returns>The configuration value associated with the specified key.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the value is required but not present.</exception>
        public string GetValue(string key, bool required = true)
        {
            string? value = _config[key];

            if (required && string.IsNullOrEmpty(value))
                throw new InvalidOperationException($"{key} is not set in environment variables or appsettings.");

            return value!;
        }
    }
}
