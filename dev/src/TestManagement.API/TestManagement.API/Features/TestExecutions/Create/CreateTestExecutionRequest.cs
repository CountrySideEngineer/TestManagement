using TestManagement.API.Features.testExecutions.Create;

namespace TestManagement.API.Features.TestExecutions.Create
{
    public class CreateTestExecutionRequest
    {
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        public string Environment { get; set; } = null!;

        public string Revision { get; set; } = null!;

        public List<TestCaseExecution> TestCases { get; set; } = new();
    }
}
