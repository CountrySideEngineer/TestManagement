using System.ComponentModel.DataAnnotations;

namespace TestManagement.API.Models
{
    public class TestReport
    {
        [Key]
        public int Id { get; set; }

        public int TestRunId { get; set; } = 0;
        public TestRun TestRun { get; set; } = new();

        public string Path { get; set; } = string.Empty;

        public string Report { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public DateTime Updateat { get; set; } = DateTime.UtcNow;
    }
}
