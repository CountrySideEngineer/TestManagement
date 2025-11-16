using System.ComponentModel.DataAnnotations;

namespace TestManagement.Model
{
    public class Tester
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(254), EmailAddress]
        public string? Email { get; set; }

        // optional affiliation
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        // Use DateTimeOffset for timezone-safe timestamps
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<TestRun> TestRuns { get; set; } = new List<TestRun>();
    }
}
