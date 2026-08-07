using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using System.IO;
using System.Net.Security;
using TestManagement.API.Infrastructure.IO;
using Xunit;
using TestIO = TestManagement.API.Infrastructure.IO;

namespace TestManagement.API.Tests
{
    public class FileReaderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("Hello, world!")]
        [InlineData("日本語のテキスト")]
        public void Read_ReturnsFileContent(string content)
        {
            var tempPath = Path.GetTempFileName();

            try
            {
                File.WriteAllText(tempPath, content);
                var sut = new TestIO.FileReader();
                var actual = sut.ReadAllText(tempPath);
                Assert.Equal(content, actual);
            }
            finally
            {
                File.Delete(tempPath);
            }
        }
    }
}
