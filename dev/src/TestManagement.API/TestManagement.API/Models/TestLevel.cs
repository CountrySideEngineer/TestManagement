using System.ComponentModel.DataAnnotations;

namespace TestManagement.API.Models
{
    public class TestLevel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property to related TestCases
        public ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();
    }
}
