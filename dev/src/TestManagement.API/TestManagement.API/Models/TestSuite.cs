using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    /// <summary>
    /// Represents a test suite which groups test cases/versions.
    /// </summary>
    public class TestSuite
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
        /// Foreign key referencing the owning <see cref="Project"/>.
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// Navigation property to the owning project. May be null for detached or DTO instances.
        /// </summary>
        [JsonIgnore]
        public virtual Project? Project { get; set; }

        /// <summary>
        /// Collection of versions (cases) that belong to this test suite.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<TestSuiteComposition>? TestSuiteCases { get; set; } = null;

        /// <summary>
        /// Compares this instance with another <see cref="TestSuite"/> for equality.
        /// Equality strategy:
        /// - If both instances have non-zero Id, compare by Id.
        /// - Otherwise, if either has a non-zero ProjectId, compare by ProjectId and Name (business key).
        /// - Otherwise return false.
        /// </summary>
        /// <param name="other">Other test suite to compare with.</param>
        /// <returns>True when considered equal; otherwise false.</returns>
        public bool Equals(TestSuite? other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other is null)
            {
                return false;
            }

            if (Id != 0 && other.Id != 0)
            {
                return Id == other.Id;
            }

            if (ProjectId != 0 || other.ProjectId != 0)
            {
                return ProjectId == other.ProjectId && string.Equals(Name, other.Name, StringComparison.Ordinal);
            }

            return false;
        }

        /// <summary>
        /// Overrides <see cref="object.Equals(object)"/> and delegates to <see cref="Equals(TestSuite)"/>.
        /// </summary>
        public override bool Equals(object? obj) => Equals(obj as TestSuite);

        /// <summary>
        /// Computes a hash code that matches the equality semantics:
        /// - If Id is non-zero, use Id's hash code.
        /// - Otherwise combine ProjectId and Name.
        /// </summary>
        /// <returns>Calculated hash code.</returns>
        public override int GetHashCode()
        {
            if (Id != 0)
            {
                return Id.GetHashCode();
            }

            unchecked
            {
                var hash = 17;
                hash = hash * 23 + ProjectId.GetHashCode();
                hash = hash * 23 + (Name != null ? StringComparer.Ordinal.GetHashCode(Name) : 0);
                return hash;
            }
        }

        /// <summary>
        /// Equality operator forwarding to <see cref="Equals(TestSuite)"/>.
        /// </summary>
        public static bool operator ==(TestSuite? left, TestSuite? right) => Equals(left, right);

        /// <summary>
        /// Inequality operator forwarding to <see cref="Equals(TestSuite)"/>.
        /// </summary>
        public static bool operator !=(TestSuite? left, TestSuite? right) => !Equals(left, right);
    }
}
