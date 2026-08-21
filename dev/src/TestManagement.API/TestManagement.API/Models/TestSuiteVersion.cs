using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a specific version of a <see cref="TestSuite"/>, containing a snapshot of included test case versions.
    /// </summary>
    public class TestSuiteVersion
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
        /// Monotonic version number for the suite version.
        /// </summary>
        public long VersionNumber { get; set; } = 0;

        /// <summary>
        /// Indicates whether this version is the latest published version for the related suite.
        /// </summary>
        public bool IsLatest { get; set; } = true;

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
    }
}
