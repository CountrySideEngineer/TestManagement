using TestManagement.APP.Interfaces;

namespace TestManagement.APP.Infrastructure
{
    /// <summary>
    /// TestResultReader is responsible for reading test result content from a file.
    /// </summary>
    public class TestResultReader : ITestResultReader
    {
        /// <summary>
        /// Read test result content from the specified file path.
        /// </summary>
        /// <param name="path">Path to file to read test results.</param>
        /// <returns>Read test result content as a string.</returns>
        public string Read(string path)
        {
            using var reader = new StreamReader(path);
            string content = reader.ReadToEnd();

            return content;
        }
    }
}
