namespace TestManagement.APP.Interfaces
{
    public interface ITestResultSource
    {
        Task<Stream> OpenAsync(CancellationToken ct = default);

        Stream Open();
    }
}
