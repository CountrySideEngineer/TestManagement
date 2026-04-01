namespace TestManagement.API.Features.testExecutions.Create
{
    public class TestCaseExecution
    {
        public string TestCaseCode { get; set; } = null!;

        public int TestCaseVersion { get; set; } = 0;

        public string TestStatusCode { get; set; } = null!;
    }

    public class CreateTestExecutionResponse
    {
        public long TestExecutionId { get; set; } = 0;

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        public string Environment { get; set; } = null!;

        public List<TestCaseExecution> TestCases { get; set; } = new();
    }
}
