using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Dto.TestExecution.Get;
using TestManagement.APP.Models;
using TestManagement.APP.Services;

namespace TestManagement.APP.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel>? _logger;

        private readonly ITestExecutionService? _testExecutionService;

        public IndexModel(
            ILogger<IndexModel>? logger,
            ITestExecutionService? testExecutionService
            ) : base()
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
        }

        public ICollection<GetTestExecutionResponse>? TestExecutions { get; set; }

        public async Task OnGetAsync()
        {
            TestExecutions = await _testExecutionService!.GetTestExecutionsAsync();
        }
    }
}

public record SummaryDto(int ErrorNum, int SkippedNum, int DisabledNum, int ExecutedNum);
public record TestRecordDto(DateTime ExecutedAt, string Result);
public record RequestTrendDto(string Time, int Count);
public record ErrorDto(string Time, string Message);
