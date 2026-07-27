namespace TestManagement.API.Infrastructure.IO
{
    public class FileReader : IFileReader
    {
        public string ReadAllText(string path) => File.ReadAllText(path).Trim();
    }
}
