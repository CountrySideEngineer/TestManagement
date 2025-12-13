namespace TestManagement.API.Infrastructure.Configuration
{
    public static class ConfigUtility
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
