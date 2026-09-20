using System.Text.Json.Serialization;
using TestManagement.API.Features.TestCases.Get;
using TestManagement.API.Models;

namespace TestManagement.API.Features.TestSuite.Get
{
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
    }
}
