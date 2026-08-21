using System.Text.Json.Serialization;

namespace TestManagement.API.Models
{
    public class TestSuite
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // FK to Project
        public long ProjectId { get; set; }

        // Navigation
        [JsonIgnore]
        public virtual Project? Project { get; set; }

        [JsonIgnore]
        public virtual ICollection<TestSuiteVersion>? TestSuiteCases { get; set; } = null;

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

        public override bool Equals(object? obj) => Equals(obj as TestSuite);

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

        public static bool operator ==(TestSuite? left, TestSuite? right) => Equals(left, right);

        public static bool operator !=(TestSuite? left, TestSuite? right) => !Equals(left, right);
    }
}
