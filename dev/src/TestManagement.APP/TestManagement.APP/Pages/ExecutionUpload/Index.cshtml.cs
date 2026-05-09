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
using TestManagement.APP.ViewModel.TestLevel;
using TestManagement.APP.ApiClients.TestLevel;
using TestManagement.APP.Services.TestLevel;

namespace TestManagement.APP.Pages.ExecutionUpload
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel>? _logger;
        private readonly ITestExecutionService? _testExecutionService;
        private readonly IEnvironmentService? _environmentService;
        private readonly ITestLevelService? _testLevelService;

        public IndexModel(
            ILogger<IndexModel>? logger,
            ITestExecutionService? testExecutionService,
            IEnvironmentService? environmentService,
            ITestLevelService? testLevelService
            )
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
            _environmentService = environmentService;
            _testLevelService = testLevelService;
        }

        [BindProperty]
        public List<IFormFile> UploadFiles { get; set; } = new List<IFormFile>();

        public ICollection<TestLevelViewModel> TestLevels { get; set; } = new List<TestLevelViewModel>();

        [BindProperty]
        public long? SelectedTestLevelId { get; set; } = 0;

        public ExecutionViewModel ExecutionViewModel { get; set; } = new ExecutionViewModel();

        public EnvironmentViewModel EnvironmentViewModel { get; set; } = new EnvironmentViewModel();

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

            TestLevels = await _testLevelService!.GetTestLevelAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadFiles.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select at least one file to upload.");
                return Page();
            }

            foreach (var fileItem in UploadFiles)
            {
                using var stream = fileItem.OpenReadStream();
                using var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(stream);
                content.Add(streamContent, "file", fileItem.FileName);

                using var reader = new StreamReader(stream);
                string fileCont = reader.ReadToEnd();

                _logger?.LogInformation(fileCont);
            }

            return Page();
            //try
            //{
            //    await _testExecutionService!.UploadTestExecutionFilesAsync(id, UploadFiles);
            //    TempData["SuccessMessage"] = "Files uploaded successfully.";
            //    return RedirectToPage("/ExecutionDetails/Index", new { id });
            //}
            //catch (Exception ex)
            //{
            //    _logger?.LogError(ex, "Error uploading files for test execution with ID {TestExecutionId}", id);
            //    ModelState.AddModelError(string.Empty, "An error occurred while uploading the files. Please try again.");
            //    return Page();
            //}
        }
    }
}
