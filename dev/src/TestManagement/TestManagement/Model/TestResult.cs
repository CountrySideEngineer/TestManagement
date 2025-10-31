using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace TestManagement.Model
{
    public enum TestStatus
    {
        Unknown = 0,
        Success = 1,
        Failure = 2,
        Skipped = 3,
        Blocked = 4
    }

    public class TestResult
    {
        public int Id { get; set; }

        public int TestRunId { get; set; }
        public TestRun? TestRun { get; set; }

        public int TestCaseId { get; set; }
        public TestCase? TestCase { get; set; }

        public TestStatus Status { get; set; } = TestStatus.Unknown;

        [MaxLength(4000)]
        public string? ActualResult { get; set; }

        public int? DurationMs { get; set; } // optional execution time    }
    }
}
