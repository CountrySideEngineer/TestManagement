using System.Text.Json.Serialization;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Features.TestSuite.Get
{
    /// <summary>
    /// Response DTO used when returning a test suite along with a collection of
    /// summarized test case versions included in that suite.
    /// </summary>
    public class GetTestSuiteResponse
    {
        /// <summary>
        /// Primary key identifier for the test suite.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Name of the test suite.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the test suite.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// UTC timestamp when the suite was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when the suite was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Collection of compact summaries for each test case version included in this suite.
        /// May be null when not loaded; callers often initialize to an empty list when composing a response.
        /// </summary>
        public List<TestCaseSummary>? TestCaseSummaries { get; set; } = null;

        /// <summary>
        /// Compact summary representing a single test case version included in a test suite.
        /// Designed for lightweight responses where full entity payloads are unnecessary.
        /// </summary>
        public class TestCaseSummary
        {
            /// <summary>
            /// Identifier of the parent test case.
            /// </summary>
            public long Id { get; set; } = 0;

            /// <summary>
            /// Version number of the test case.
            /// </summary>
            public long VersionNumber { get; set; } = 0;

            /// <summary>
            /// Human-readable name of the test case version.
            /// </summary>
            public string Name { get; set; } = string.Empty;

            /// <summary>
            /// Description for the test case version.
            /// </summary>
            public string Description { get; set; } = string.Empty;

            /// <summary>
            /// Display name of the test level associated with this test case version (e.g. "Unit", "Integration").
            /// </summary>
            public string TestLevelName { get; set; } = string.Empty;
        }
    }
}
