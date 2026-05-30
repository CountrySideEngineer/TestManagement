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
using TestManagement.APP.Services.TestExecution.Import;
using TestManagement.APP.Infrastructure.TestResultSource;
using TestManagement.APP.Parse;
using TestManagement.APP.Dto.TestResult.Import;

namespace TestManagement.APP.Pages.ExecutionUpload
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel>? _logger;
        private readonly ITestExecutionService? _testExecutionService;
        private readonly IEnvironmentService? _environmentService;
        private readonly ITestLevelService? _testLevelService;
        private readonly IImportTestResultService? _importTestResultService;

        public IndexModel(
            ILogger<IndexModel>? logger,
            ITestExecutionService? testExecutionService,
            IEnvironmentService? environmentService,
            ITestLevelService? testLevelService,
            IImportTestResultService? importTestResultService
            )
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
            _environmentService = environmentService;
            _testLevelService = testLevelService;
            _importTestResultService = importTestResultService;
        }

        [BindProperty]
        public long EnvId { get; set; } = 0;

        [BindProperty]
        public List<IFormFile> UploadFiles { get; set; } = new List<IFormFile>();

        public ICollection<TestLevelViewModel> TestLevels { get; set; } = new List<TestLevelViewModel>();

        [BindProperty]
        public long? SelectedTestLevelId { get; set; } = 0;

        public ExecutionViewModel ExecutionViewModel { get; set; } = new ExecutionViewModel();

        public EnvironmentViewModel EnvironmentViewModel { get; set; } = new EnvironmentViewModel();

        public async Task OnGetAsync(long id)
        {
            EnvId = id;
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

        public async Task<IActionResult> OnPostAsync(long? id, long? selectedTestLevelId)
        {
            // If id is provided as a route/form value, use it to populate EnvId so it is available during POST handling
            if (id.HasValue)
            {
                EnvId = id.Value;
            }

            // If selectedTestLevelId is provided as a route/form value, populate the bound property
            if (selectedTestLevelId.HasValue)
            {
                SelectedTestLevelId = selectedTestLevelId.Value;
            }
            if (UploadFiles.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select at least one file to upload.");
                return Page();
            }

            foreach (var fileItem in UploadFiles)
            {
                try
                {
                    // Wrap uploaded file in a test result source and use a parser to import
                    var source = new FormFileTestResultSource(fileItem);
                    var parser = new GTestXmlResultParser();

                    var request = new ImportTestResultRequest
                    {
                        Source = source,
                        Parser = parser
                    };

                    if (_importTestResultService is null)
                    {
                        _logger?.LogError("IImportTestResultService is not available via DI.");
                        ModelState.AddModelError(string.Empty, "Import service is not available.");
                        return Page();
                    }

                    long testLevelId = SelectedTestLevelId ?? 0;
                    await _importTestResultService.ImportAsync(EnvId, testLevelId, request);
                    _logger?.LogInformation("Imported test results from uploaded file {FileName}", fileItem.FileName);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error importing uploaded file {FileName}", fileItem.FileName);
                    ModelState.AddModelError(string.Empty, $"Failed to import file {fileItem.FileName}: {ex.Message}");
                    return Page();
                }
            }

            TempData["SuccessMessage"] = "Files imported successfully.";
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
