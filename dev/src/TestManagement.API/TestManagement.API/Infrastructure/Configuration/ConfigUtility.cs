namespace TestManagement.API.Infrastructure.Configuration
{
    /// <summary>
    /// Utility methods for reading configuration and environment values.
    /// </summary>
    public static class ConfigUtility
    {
        /// <summary>
        /// Retrieves a configuration value by key and optionally enforces it to be present.
        /// </summary>
        /// <param name="config">The application configuration instance to read values from.</param>
        /// <param name="key">The configuration key or environment variable name to look up.</param>
        /// <param name="required">If true, throws an exception when the value is null or empty.</param>
        /// <returns>The configuration value string. When <paramref name="required"/> is true,
        /// this method will never return null or empty (it will throw instead).</returns>
        /// <exception cref="InvalidOperationException">Thrown when a required value is missing.</exception>
        public static string GetValue(IConfiguration config, string key, bool required = true)
        {
            string? value = config[key];

            if (required && string.IsNullOrEmpty(value))
                throw new InvalidOperationException($"{key} is not set in environment variables or appsettings.");

            return value!;
        }
    }
}
