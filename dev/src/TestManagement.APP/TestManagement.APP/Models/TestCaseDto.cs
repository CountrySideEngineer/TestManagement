using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestManagement.APP.Models
{
    /// <summary>
    /// Data transfer object that represents a test case.
    /// </summary>
    public class TestCaseDto
    {
        /// <summary>
        /// Primary identifier for the test case.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Short title describing the test case.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the test case.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// UTC timestamp when the test case was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when the test case was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Foreign key referencing the associated test level.
        /// </summary>
        public int TestLevelId { get; set; }

        /// <summary>
        /// Navigation property to the related test level DTO. May be null.
        /// </summary>
        public TestLevelDto? TestLevel { get; set; }
    }
}
