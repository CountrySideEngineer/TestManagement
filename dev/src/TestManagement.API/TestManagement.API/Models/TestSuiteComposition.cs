using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a specific version of a <see cref="TestSuite"/>, containing a snapshot of included test case versions.
    /// </summary>
    public class TestSuiteComposition
    {
        /// <summary>
        /// Primary key identifier for the test suite version.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Foreign key referencing the owning <see cref="TestSuite"/>.
        /// </summary>
        public long TestSuiteId { get; set; }

        /// <summary>
        /// Foreign key referencing the associated <see cref="TestCaseVersion"/>.
        /// Identifies which specific version of a test case is included in this suite composition.
        /// </summary>
        public long TestCaseVersionId { get; set; }

        /// <summary>
        /// UTC timestamp when this version record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// UTC timestamp when this version record was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Collection of test case versions included in this suite version.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<TestCaseVersion>? TestCaseVersions { get; set; } = null;

        /// <summary>
        /// Navigation back to the owning <see cref="TestSuite"/>.
        /// </summary>
        [JsonIgnore]
        public virtual TestSuite? TestSuite { get; set; }

        /// <summary>
        /// Navigation property to the parent <see cref="TestCase"/>.
        /// May be null for detached instances or DTOs.
        /// </summary>
        [JsonIgnore]
        public virtual TestCase? TestCase { get; set; }
    }
}
