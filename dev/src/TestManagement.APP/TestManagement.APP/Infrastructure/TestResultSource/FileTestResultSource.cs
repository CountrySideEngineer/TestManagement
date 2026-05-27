using TestManagement.APP.Interfaces;

namespace TestManagement.APP.Infrastructure.TestResultSource
{
    public class FileTestResultSource : ITestResultSource
    {
        public Stream Open(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The file at path '{path}' was not found.");
            }

            Stream stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                8192,
                false);

            return stream;
        }

        public Task<Stream> OpenAsync(string path, CancellationToken ct = default)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The file at path '{path}' was not found.");
            }

            Stream stream = new FileStream(
                path, 
                FileMode.Open, 
                FileAccess.Read, 
                FileShare.Read, 
                8192, 
                true);

            return Task.FromResult(stream);
        }
    }
}
