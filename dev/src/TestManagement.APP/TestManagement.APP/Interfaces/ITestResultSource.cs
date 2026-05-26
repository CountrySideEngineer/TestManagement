namespace TestManagement.APP.Interfaces
{
    public interface ITestResultSource
    {
        Task<Stream> OpenAsync(string path);

        Stream Open(string path);
    }
}
