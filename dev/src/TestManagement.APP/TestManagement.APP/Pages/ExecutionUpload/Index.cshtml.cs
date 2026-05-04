using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Services.TestExecution;
using TestManagement.APP.Dto.TestExecution.Get;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using TestManagement.APP.ViewModel.Executions;

namespace TestManagement.APP.Pages.ExecutionUpload
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel>? _logger;
        private readonly ITestExecutionService? _testExecutionService;

        public IndexModel(
            ILogger<IndexModel>? logger, 
            ITestExecutionService? testExecutionService)
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
        }

        [BindProperty]
        public List<IFormFile> UploadFiles { get; set; } = new List<IFormFile>();

        [BindProperty]
        public string Revision { get; set; } = string.Empty;

        [BindProperty]
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        [BindProperty]
        public string Environment { get; set; } = string.Empty;

        public ExecutionViewModel ExecutionViewModel { get; set; } = new ExecutionViewModel();

        public ICollection<GetTestExecutionResponse>? TestExecutions { get; set; }

        public async Task OnGetAsync(long id)
        {
            var testExecution = await _testExecutionService!.GetTestExecutionByIdAsync(id);
            if (testExecution is not null)
            {
                ExecutionViewModel = testExecution;
            }
        }
    }
}
