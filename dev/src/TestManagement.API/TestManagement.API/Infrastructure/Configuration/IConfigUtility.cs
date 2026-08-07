namespace TestManagement.API.Infrastructure.Configuration
{
    public interface IConfigUtility
    {
        string GetValue(string key, bool required = true);   
    }
}
