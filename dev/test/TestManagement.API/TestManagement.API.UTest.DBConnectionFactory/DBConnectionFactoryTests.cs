using Moq;
using TestManagement.API.Infrastructure.Configuration;
using TestManagement.API.Infrastructure.IO;
using TestManagement.API.Infrastructure.Database;

namespace TestManagement.API.Tests
{
    public class DBConnectionFactoryTests
    {
        [Theory]
        [InlineData("localhost", "5432", "mydb", "dbuser", "/secrets/dbpass", "s3cr3t")]
        public void CreatePostgresConnectionString_ValidValues_ContainsExpectedParts(
            string host,
            string port,
            string database,
            string user,
            string passFilePath,
            string password)
        {
            var mockConfig = new Mock<IConfigUtility>();
            mockConfig.Setup(c => c.GetValue("DB_HOST", true)).Returns(host);
            mockConfig.Setup(c => c.GetValue("DB_PORT", true)).Returns(port);
            mockConfig.Setup(c => c.GetValue("DB_NAME", true)).Returns(database);
            mockConfig.Setup(c => c.GetValue("DB_USER", true)).Returns(user);
            mockConfig.Setup(c => c.GetValue("DB_PASSWORD_FILE", true)).Returns(passFilePath);

            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(f => f.ReadAllText(passFilePath)).Returns(password);

            var factory = new DBConnectionFactory(mockConfig.Object, mockFileReader.Object);

            string connectionString = factory.CreatePostgresConnectionString();

            Assert.Contains($"Host={host}", connectionString);
            Assert.Contains($"Port={port}", connectionString);
            Assert.Contains($"Database={database}", connectionString);
            Assert.Contains($"Username={user}", connectionString);
            Assert.Contains($"Password={password}", connectionString);
        }

        [Theory]
        [InlineData("localhost", "not-a-number", "mydb", "dbuser", "/secrets/dbpass", "pw")]
        public void CreatePostgresConnectionString_InvalidPort_ThrowsFormatException(
            string host,
            string port,
            string database,
            string user,
            string passFilePath,
            string password)
        {
            var mockConfig = new Mock<IConfigUtility>();
            mockConfig.Setup(c => c.GetValue("DB_HOST", true)).Returns(host);
            mockConfig.Setup(c => c.GetValue("DB_PORT", true)).Returns(port);
            mockConfig.Setup(c => c.GetValue("DB_NAME", true)).Returns(database);
            mockConfig.Setup(c => c.GetValue("DB_USER", true)).Returns(user);
            mockConfig.Setup(c => c.GetValue("DB_PASSWORD_FILE", true)).Returns(passFilePath);

            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(f => f.ReadAllText(passFilePath)).Returns(password);

            var factory = new DBConnectionFactory(mockConfig.Object, mockFileReader.Object);

            Assert.Throws<FormatException>(() => factory.CreatePostgresConnectionString());
        }

        [Theory]
        [InlineData("DB_HOST")]
        [InlineData("DB_PORT")]
        [InlineData("DB_NAME")]
        [InlineData("DB_USER")]
        [InlineData("DB_PASSWORD_FILE")]
        public void CreatePostgresConnectionString_MissingRequiredConfig_ThrowsInvalidOperationException(string missingKey)
        {
            var mockConfig = new Mock<IConfigUtility>();

            // デフォルトで有効な値を返す設定
            mockConfig.Setup(c => c.GetValue("DB_HOST", true)).Returns("localhost");
            mockConfig.Setup(c => c.GetValue("DB_PORT", true)).Returns("5432");
            mockConfig.Setup(c => c.GetValue("DB_NAME", true)).Returns("mydb");
            mockConfig.Setup(c => c.GetValue("DB_USER", true)).Returns("dbuser");
            mockConfig.Setup(c => c.GetValue("DB_PASSWORD_FILE", true)).Returns("/secrets/dbpass");

            // 指定されたキーだけ例外を投げる
            mockConfig.Setup(c => c.GetValue(missingKey, true)).Throws(new InvalidOperationException($"{missingKey} is not set"));

            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns("pw");

            var factory = new DBConnectionFactory(mockConfig.Object, mockFileReader.Object);

            Assert.Throws<InvalidOperationException>(() => factory.CreatePostgresConnectionString());
        }
    }
}
