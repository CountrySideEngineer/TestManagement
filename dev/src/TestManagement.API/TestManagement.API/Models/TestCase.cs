using System.ComponentModel.DataAnnotations;

namespace TestManagement.API.Models
{
    public class TestCase
    {
        private readonly List<TestCaseVersion> _versions = new();

        [Key]
        public int Id { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get;set; } = DateTime.UtcNow;

        public IReadOnlyCollection<TestCaseVersion> Versions => _versions;

    }
}
