namespace TestManagement.APP.Models
{
    public enum TestStatus
    {
        Unknown = 0,
        Success = 1,
        Failure = 2,
        Skipped = 3,
        Blocked = 4
    }

    public class TestResultDto
    {
        public int Id { get; set; }
        
        public string ActualResult { get; set; } = string.Empty;

        public int TestRunId { get; set; }

        public int TestCaseId { get; set; }
        public TestCaseDto? TestCase { get; set; }

        public TestStatus Status { get; set; } = TestStatus.Unknown;

        public DateTime ExecutedAt { get; set; } = DateTime.MinValue;
    }
}
