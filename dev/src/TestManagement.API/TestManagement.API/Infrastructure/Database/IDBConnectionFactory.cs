namespace TestManagement.API.Infrastructure.Database
{
    public interface IDBConnectionFactory
    {
        string CreatePostgresConnectionString();
    }
}
