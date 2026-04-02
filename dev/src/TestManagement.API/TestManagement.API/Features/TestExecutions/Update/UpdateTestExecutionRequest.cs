using TestManagement.API.Features.testExecutions.Create;

namespace TestManagement.API.Features.TestExecutions.Update
{
    public class UpdateTestExecutionRequest
    {
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        public string Environment { get; set; } = string.Empty;

        public string Revision { get; set; } = string.Empty;

        public List<TestCaseExecution> TestCases { get; set; } = new();
    }
}
