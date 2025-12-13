using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        // Navigation property (reference to parent table.)
        [BindNever]
        public TestCase? TestCase { get; set; }
    }
}
