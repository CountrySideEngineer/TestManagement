using TestManagement.API.Features.TestExecutions.Create;

namespace TestManagement.API.Features.TestExecutions.Update
{
    public class UpdateTestExecutionResponse
    {
            public long TestExecutionId { get; set; } = 0;
    
            public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    
            public string Environment { get; set; } = null!;
    
            public string Revision { get; set; } = string.Empty;
    
            public List< TestCaseExecution> TestCases { get; set; } = new();
    }
}
