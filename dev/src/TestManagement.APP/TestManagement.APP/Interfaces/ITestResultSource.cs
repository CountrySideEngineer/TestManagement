namespace TestManagement.APP.Interfaces
{
    public interface ITestResultSource
    {
        Task<Stream> OpenAsync(string path, CancellationToken ct = default);

        Stream Open(string path);
    }
}
