using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestManagement.Models
{
    public class TestRun
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(TestCase))]
        public int TestCaseId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        // Navigation property (reference to parent table.)
        public TestCase TestCase { get; set; } = new TestCase();
    }
}
