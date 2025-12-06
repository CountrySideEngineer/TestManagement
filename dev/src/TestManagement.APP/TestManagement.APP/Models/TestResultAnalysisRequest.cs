using System.ComponentModel.DataAnnotations;

namespace TestManagement.APP.Models
{
    public enum Status
    {
        NotStarted,
        Running,
        Finished
    };

    public enum Result
    {
        Unknown,
        Passed,
        Failed
    };

    public class TestResultAnalysisRequest
    {
        [Key]
        public int Key { get; set; }

        [Required]
        public string DirectoryPath { get; set; } = string.Empty;

        public Status Status { get;set; } = Status.NotStarted;

        public Result Result { get;set; } = Result.Unknown;

        public DateTime RegisteredAt { get;set; } = DateTime.UtcNow;

        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
