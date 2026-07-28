using System;
using System.Linq;
using System.Threading.Tasks;
using TestManagement.API.Models.Report.Xml;
using TestManagement.API.Services.Xml;
using Xunit;

namespace TestManagement.API.UTests
{
    public class TestResultXmlConverterTests
    {
        [Fact]
        public async Task ConvertAsync_NullSuites_ReturnsEmpty()
        {
            var converter = new TestResultXmlConverter();

            var results = await converter.ConvertAsync(null);

            Assert.Empty(results);
        }

        [Theory]
        [InlineData("result text", "result text")]
        [InlineData("", "")]
        public async Task ConvertAsync_NoFailure_MapsActualResultAndExecutedAt(string resultValue, string expectedActual)
        {
            // Arrange
            var timestamp = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            var testCase = new TestCaseXml
            {
                Result = resultValue,
                Timestamp = timestamp,
                Failure = null
            };

            var suite = new TestSuiteXml();
            suite.TestCases.Add(testCase);

            var suites = new TestSuitesXml();
            suites.TestItems.Add(suite);

            var converter = new TestResultXmlConverter();

            // Act
            var results = await converter.ConvertAsync(suites);

            // Assert
            Assert.Single(results);
            var r = results.Single();

            Assert.Equal(expectedActual, r.ActualResult);
            Assert.Equal(timestamp, r.ExecutedAt);
            Assert.NotNull(r.Status);

            var now = DateTime.UtcNow;
            Assert.True((now - r.CreatedAt).TotalSeconds < 10, $"CreatedAt too old: {r.CreatedAt:o}");
            Assert.True((now - r.UpdatedAt).TotalSeconds < 10, $"UpdatedAt too old: {r.UpdatedAt:o}");
        }

        [Theory]
        [InlineData("failure message", "result ignored", "failure message")]
        [InlineData("another failure", "", "another failure")]
        public async Task ConvertAsync_WithFailure_UsesFailureMessage(string failureMessage, string resultValue, string expectedActual)
        {
            // Arrange
            var timestamp = new DateTime(2021, 6, 2, 8, 30, 0, DateTimeKind.Utc);

            var testCase = new TestCaseXml
            {
                Result = resultValue,
                Timestamp = timestamp,
                Failure = new Failure { Message = failureMessage }
            };

            var suite = new TestSuiteXml();
            suite.TestCases.Add(testCase);

            var suites = new TestSuitesXml();
            suites.TestItems.Add(suite);

            var converter = new TestResultXmlConverter();

            // Act
            var results = await converter.ConvertAsync(suites);

            // Assert
            Assert.Single(results);
            var r = results.Single();

            Assert.Equal(expectedActual, r.ActualResult);
            Assert.Equal(timestamp, r.ExecutedAt);
            Assert.NotNull(r.Status);

            var now = DateTime.UtcNow;
            Assert.True((now - r.CreatedAt).TotalSeconds < 10, $"CreatedAt too old: {r.CreatedAt:o}");
            Assert.True((now - r.UpdatedAt).TotalSeconds < 10, $"UpdatedAt too old: {r.UpdatedAt:o}");
        }
    }
}
