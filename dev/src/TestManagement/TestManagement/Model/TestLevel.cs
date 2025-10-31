using System.ComponentModel.DataAnnotations;

namespace TestManagement.Model
{
    public class TestLevel
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty; // e.g. "Unit", "Integration"

        [MaxLength(1000)]
        public string? Description { get; set; }

        public ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();
    }
}
