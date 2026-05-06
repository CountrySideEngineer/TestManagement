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
using TestManagement.APP.Services.Environment;
using TestManagement.APP.ViewModel.Environment;
using System.Diagnostics.Eventing.Reader;

namespace TestManagement.APP.Pages.ExecutionUpload
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel>? _logger;
        private readonly ITestExecutionService? _testExecutionService;
        private readonly IEnvironmentService? _environmentService;

        public IndexModel(
            ILogger<IndexModel>? logger, 
            ITestExecutionService? testExecutionService,
            IEnvironmentService? environmentService
            )
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
            _environmentService = environmentService;
        }

        [BindProperty]
        public List<IFormFile> UploadFiles { get; set; } = new List<IFormFile>();

        public ExecutionViewModel ExecutionViewModel { get; set; } = new ExecutionViewModel();

        public EnvironmentViewModel EnvironmentViewModel { get;set; } = new EnvironmentViewModel();

        public async Task OnGetAsync(long id)
        {
            var testExecution = await _testExecutionService!.GetTestExecutionByIdAsync(id);
            if (testExecution is not null)
            {
                ExecutionViewModel = testExecution;
            }
            string envName = ExecutionViewModel.Environment;
            var env = await _environmentService!.GetLatestEnvironmentByNameAsync(envName);
            if (env is not null)
            {
                EnvironmentViewModel = env;
            }
        }
    }
}
