namespace TestManagement.API.Infrastructure.IO
{
    public interface IFileReader
    {
        string ReadAllText(string path);
    }
}
