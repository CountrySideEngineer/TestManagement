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
        private readonly IFormFile _file;

        public FormFileTestResultSource(IFormFile file)
        {
            _file = file ?? throw new ArgumentNullException(nameof(file));
        }

        public Stream Open()
        {
            return _file.OpenReadStream();
        }

        public Task<Stream> OpenAsync(CancellationToken ct = default)
        {
            // IFormFile.OpenReadStream is a synchronous API, return it wrapped in a completed Task.
            Stream stream = _file.OpenReadStream();
            return Task.FromResult(stream);
        }
    }
}
