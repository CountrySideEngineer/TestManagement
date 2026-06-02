using TestManagement.APP.Interfaces;

namespace TestManagement.APP.Infrastructure.TestResultSource
{
    /// <summary>
    /// Concrete implementation of ITestResultSource that reads test results from a file.
    /// </summary>
    public class FileTestResultSource : ITestResultSource
    {
        /// <summary>
        /// Path to the file containing the test results.
        /// This should be set through the constructor and is immutable after that.
        /// </summary>
        private readonly string _path = string.Empty;

        /// <summary>
        /// Constructs a new instance of FileTestResultSource with the specified file path.
        /// </summary>
        /// <param name="path">The path to the file containing the test results.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided path is null.</exception>
        public FileTestResultSource(string path)
        {
            this._path = path ?? throw new ArgumentNullException(nameof(path));
        }

        /// <summary>
        /// Opens a stream to read the test results from the specified file path.
        /// </summary>
        /// <returns>A stream for reading the test results.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file at the specified path does not exist.</exception>
        public Stream Open()
        {
            if (!File.Exists(_path))
            {
                throw new FileNotFoundException($"The file at path '{_path}' was not found.");
            }

            Stream stream = new FileStream(
                _path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                8192,
                false);

            return stream;
        }

        /// <summary>
        /// Opens a stream to read the test results from the specified file path asynchronously.
        /// </summary>
        /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a stream for reading the test results.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file at the specified path does not exist.</exception>
        public Task<Stream> OpenAsync(CancellationToken ct = default)
        {
            if (!File.Exists(_path))
            {
                throw new FileNotFoundException($"The file at path '{_path}' was not found.");
            }

            Stream stream = new FileStream(
                _path, 
                FileMode.Open, 
                FileAccess.Read, 
                FileShare.Read, 
                8192, 
                true);

            return Task.FromResult(stream);
        }
    }
}
