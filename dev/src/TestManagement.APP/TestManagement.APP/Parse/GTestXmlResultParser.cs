using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Xml.Linq;
using TestManagement.APP.Dto.TestResult;
using TestManagement.APP.Interfaces;

namespace TestManagement.APP.Parse
{
    /// <summary>
    /// Parser for Google Test XML result files. This class converts XML test case
    /// elements into <see cref="ParsedTestResult"/> objects.
    /// </summary>
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

            ICollection<ParsedTestResult> results = ParseTestCases(testCaseElements).ToList();
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

            var result = new ParsedTestResult()
            {
                Code = $"{className}::{name}",
                Name = name,
                Description = className,
                ExecutedAt = executedAt,
            };
            ParseTestCaseResult(testCaseElement, result);
            result.StatusCode = result.IsFailed ? "Failed" : (result.IsStatusRun ? "Passed" : "Not Run");

            return result;
        }

        /// <summary>
        /// Parses status and result attributes on a test case XML element and updates
        /// the provided <see cref="ParsedTestResult"/> instance with the derived
        /// status flags (run, completed, skipped, suppressed and failed).
        /// </summary>
        /// <param name="testCaseElement">The XML element that represents a single test case.</param>
        /// <param name="testResult">The parsed test result instance to update.</param>
        protected virtual void ParseTestCaseResult(XElement testCaseElement, ParsedTestResult testResult)
        {
            string status = testCaseElement.Attribute("status")?.Value ?? string.Empty;
            string result = testCaseElement.Attribute("result")?.Value ?? string.Empty;

            if ("run" == status)
            {
                // Test case was executed, determine pass/fail based on result
                if ("completed" == result)
                {
                    testResult.IsStatusRun = true;
                    testResult.IsResultCompleted = true;
                    testResult.IsResultSkipped = false;
                    testResult.IsResultSuppressed = false;

                    if (null == testCaseElement.Element("failure"))
                    {
                        testResult.IsFailed = false;
                    }
                    else
                    {
                        testResult.IsFailed = true;
                    }
                }
                else if ("skipped" == result)
                {
                    testResult.IsStatusRun = true;
                    testResult.IsResultCompleted = false;
                    testResult.IsResultSkipped = true;
                    testResult.IsResultSuppressed = false;
                    testResult.IsFailed = false;
                }
                else
                {
                    throw new InvalidDataException();
                }
            }
            else if ("notrun" == status)
            {
                if ("suppressed" == result)
                {
                    testResult.IsStatusRun = false;
                    testResult.IsResultCompleted = false;
                    testResult.IsResultSkipped = false;
                    testResult.IsResultSuppressed = true;
                    testResult.IsFailed = false;
                }
                else
                {
                    throw new InvalidDataException();
                }
            }
        }

        /// <summary>
        /// Asynchronously parses Google Test XML content and returns a collection of
        /// <see cref="ParsedTestResult"/> objects representing the parsed test cases.
        /// </summary>
        /// <param name="content">The XML content to parse.</param>
        /// <param name="ct">A cancellation token to cancel the operation.</param>
        /// <returns>A task that resolves to a collection of parsed test results.</returns>
        public virtual async Task<ICollection<ParsedTestResult>> ParseAsync(string content, CancellationToken ct = default)
        {
            var results = await Task.Run<ICollection<ParsedTestResult>>(() =>
            {
                XDocument xdoc = XDocument.Parse(content);
                IEnumerable<XElement> testCaseElements = xdoc.Descendants("testcase");

                ICollection<ParsedTestResult> results = ParseTestCases(testCaseElements).ToList();

                return results;
            }, ct);

            return results;
        }

        /// <summary>
        /// Asynchronously reads the provided stream and parses its XML content into
        /// a collection of <see cref="ParsedTestResult"/> instances.
        /// </summary>
        /// <param name="stream">Stream containing the XML content.</param>
        /// <param name="ct">A cancellation token to cancel the operation.</param>
        /// <returns>A task that resolves to a collection of parsed test results.</returns>
        public virtual async Task<ICollection<ParsedTestResult>> ParseAsync(Stream stream, CancellationToken ct = default)
        {
            using var reader = new StreamReader(stream);
            string content = await reader.ReadToEndAsync();

            var results = await ParseAsync(content, ct);

            return results;
        }

        /// <summary>
        /// Synchronously reads the provided stream and parses its XML content into
        /// a collection of <see cref="ParsedTestResult"/> instances.
        /// </summary>
        /// <param name="stream">Stream containing the XML content to parse.</param>
        /// <returns>Collection of parsed test results.</returns>
        public virtual ICollection<ParsedTestResult> Parse(Stream stream)
        {
            using var reader = new StreamReader(stream);
            string content = reader.ReadToEnd();

            ICollection<ParsedTestResult> results = Parse(content);

            return results;
        }
    }
}
