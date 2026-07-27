namespace TestManagement.API.Infrastructure.IO
{
    /// <summary>
    /// Provides helper methods to read files from disk.
    /// </summary>
    public class FileReader : IFileReader
    {
        /// <summary>
        /// Reads all text from the specified file path and trims surrounding whitespace.
        /// </summary>
        /// <param name="path">The file system path to read.</param>
        /// <returns>The file contents with leading and trailing whitespace removed.</returns>
        /// <exception cref="System.IO.IOException">Thrown when an I/O error occurs while reading the file.</exception>
        public string ReadAllText(string path) => File.ReadAllText(path).Trim();
    }
}
