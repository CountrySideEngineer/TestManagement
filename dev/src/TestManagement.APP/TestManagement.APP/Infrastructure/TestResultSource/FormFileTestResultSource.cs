using Microsoft.AspNetCore.Http;
using TestManagement.APP.Interfaces;
using System.Threading;

namespace TestManagement.APP.Infrastructure.TestResultSource
{
    /// <summary>
    /// ITestResultSource implementation for uploaded form files (`IFormFile`).
    /// Provides synchronous and asynchronous access to the underlying file stream.
    /// </summary>
    public class FormFileTestResultSource : ITestResultSource
    {
        /// <summary>
        /// The underlying uploaded form file provided by the caller.
        /// </summary>
        private readonly IFormFile _file;

        /// <summary>
        /// Initializes a new instance of <see cref="FormFileTestResultSource"/> with the specified <see cref="IFormFile"/>.
        /// </summary>
        /// <param name="file">The uploaded form file to use as the test result source.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <c>null</c>.</exception>
        public FormFileTestResultSource(IFormFile file)
        {
            _file = file ?? throw new ArgumentNullException(nameof(file));
        }

        /// <summary>
        /// Opens a readable <see cref="Stream"/> for the underlying form file.
        /// This method returns a synchronous stream from <see cref="IFormFile.OpenReadStream"/>.
        /// Caller is responsible for disposing the returned stream when finished.
        /// </summary>
        /// <returns>A readable <see cref="Stream"/> for the file content.</returns>
        public Stream Open()
        {
            return _file.OpenReadStream();
        }

        /// <summary>
        /// Asynchronously opens a readable <see cref="Stream"/> for the underlying form file.
        /// Note: <see cref="IFormFile.OpenReadStream"/> is a synchronous API; this method wraps the synchronous stream in a completed <see cref="Task"/>.
        /// Caller is responsible for disposing the returned stream when finished.
        /// </summary>
        /// <param name="ct">A <see cref="CancellationToken"/> that is currently unused but provided for API compatibility.</param>
        /// <returns>A <see cref="Task{Stream}"/> that returns a readable <see cref="Stream"/> for the file content.</returns>
        public Task<Stream> OpenAsync(CancellationToken ct = default)
        {
            // IFormFile.OpenReadStream is a synchronous API, return it wrapped in a completed Task.
            Stream stream = _file.OpenReadStream();
            return Task.FromResult(stream);
        }
    }
}
