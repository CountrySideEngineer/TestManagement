using System.ComponentModel.DataAnnotations;

namespace TestManagement.Model
{
    public class Tester
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(254), EmailAddress]
        public string? Email { get; set; }

        // optional affiliation
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        public ICollection<TestRun> TestRuns { get; set; } = new List<TestRun>();
    }
}
