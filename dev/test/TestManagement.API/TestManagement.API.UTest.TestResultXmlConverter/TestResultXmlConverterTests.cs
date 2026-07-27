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
        [InlineData("result text", "", "result text")]
        [InlineData("", "failure message", "failure message")]
        [InlineData("", "", "")]
        public async Task ConvertAsync_MapsActualResultAndExecutedAt(string resultValue, string failureMessage, string expectedActual)
        {
            // Arrange
            var timestamp = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            var testCase = new TestCaseXml
            {
                Result = resultValue,
                Timestamp = timestamp
            };

            if (!string.IsNullOrEmpty(failureMessage))
            {
                testCase.Failure = new Failure { Message = failureMessage };
            }

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

            // MapStatus currently returns an instance; ensure it's not null.
            Assert.NotNull(r.Status);

            // CreatedAt/UpdatedAt should be set to near-now (allow some slack).
            var now = DateTime.UtcNow;
            Assert.True((now - r.CreatedAt).TotalSeconds < 10, $"CreatedAt too old: {r.CreatedAt:o}");
            Assert.True((now - r.UpdatedAt).TotalSeconds < 10, $"UpdatedAt too old: {r.UpdatedAt:o}");
        }
    }
}