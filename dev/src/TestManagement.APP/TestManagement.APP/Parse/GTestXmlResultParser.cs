using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Xml.Linq;
using TestManagement.APP.Dto.TestResult;
using TestManagement.APP.Interfaces;

namespace TestManagement.APP.Parse
{
    public class GTestXmlResultParser : ITestResultParser
    {
        /// <summary>
        /// Parses the given XML content of Google Test results and extracts test case information
        /// to create a collection of ParsedTestResult objects.
        /// </summary>
        /// <param name="content">XML content of Google Test results.</param>
        /// <returns>Collection of ParsedTestResult objects.</returns>
        /// <remarks>
        /// - Ensure that the XML content is well-formed and contains the expected structure for Google Test results.
        /// - Handle any potential exceptions that may arise during XML parsing, such as malformed XML or missing attributes,
        ///   and consider logging these exceptions for debugging purposes.
        /// </remarks>
        public ICollection<ParsedTestResult> Parse(string content)
        {
            XDocument xdoc = XDocument.Parse(content);
            IEnumerable<XElement> testCaseElements = xdoc.Descendants("testcase");

            ICollection<ParsedTestResult> results = [.. ParseTestCases(testCaseElements)];
            return results;
        }

        /// <summary>
        /// Parses the given collection of test case XML elements and converts them
        /// into ParsedTestResult objects.
        /// </summary>
        /// <param name="testCaseElements">Collection of test case XML elements.</param>
        /// <returns>Collection of ParsedTestResult objects.</returns>
        protected virtual IEnumerable<ParsedTestResult> ParseTestCases(IEnumerable<XElement> testCaseElements)
        {
            foreach (var testCaseElement in testCaseElements)
            {
                yield return ParseTestCase(testCaseElement);
            }
        }
        
        /// <summary>
        /// Parses the given test case XML element and converts it into a ParsedTestResult object.
        /// </summary>
        /// <param name="testCaseElement">Test case XML element.</param>
        /// <returns>ParsedTestResult object.</returns>
        /// <remarks>Add processing to check whether the XML has the required attribute,
        /// and thrown an exception if it does not.</remarks>
        protected virtual ParsedTestResult ParseTestCase(XElement testCaseElement)
        {
            string name = testCaseElement.Attribute("name")?.Value ?? string.Empty;
            string className = testCaseElement.Attribute("classname")?.Value ?? string.Empty;

            DateTime executedAt = DateTime.UtcNow;
            string executedAtStr = testCaseElement.Attribute("timestamp")?.Value ?? string.Empty;
            if (!string.IsNullOrEmpty(executedAtStr) && DateTime.TryParse(executedAtStr, out executedAt))
            {
                executedAt = DateTime.SpecifyKind(executedAt, DateTimeKind.Utc);
            }

            bool isFailed = testCaseElement.Element("failure") != null;

            ParsedTestResult result = new ParsedTestResult
            {
                Code = $"{className}::{name}",
                Name = name,
                Description = className,
                ExecutedAt = executedAt,
                IsFailed = isFailed
            };

            return result;
        }

        public async Task<ICollection<ParsedTestResult>> ParseAsync(string content)
        {
            var results = await Task.Run<ICollection<ParsedTestResult>>(() =>
            {
                XDocument xdoc = XDocument.Parse(content);
                IEnumerable<XElement> testCaseElements = xdoc.Descendants("testcase");

                ICollection<ParsedTestResult> results = [.. ParseTestCases(testCaseElements)];

                return results;
            });

            return results;
        }
    }
}
