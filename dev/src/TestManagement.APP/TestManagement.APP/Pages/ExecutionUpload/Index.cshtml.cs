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
    /// <summary>
    /// Page model for the execution upload page. Handles displaying execution details,
    /// binding uploaded files and invoking the import pipeline for test results.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Logger instance for logging page-level events and errors.
        /// </summary>
        private readonly ILogger<IndexModel>? _logger;

        /// <summary>
        /// Service for retrieving test execution information.
        /// </summary>
        private readonly ITestExecutionService? _testExecutionService;

        /// <summary>
        /// Service for retrieving environment information.
        /// </summary>
        private readonly IEnvironmentService? _environmentService;

        /// <summary>
        /// Service for retrieving available test levels.
        /// </summary>
        private readonly ITestLevelService? _testLevelService;

        /// <summary>
        /// Service responsible for importing test results from provided sources.
        /// </summary>
        private readonly IImportTestResultService? _importTestResultService;

        /// <summary>
        /// Constructs an instance of <see cref="IndexModel"/> with required services.
        /// </summary>
        /// <param name="logger">Logger for diagnostics.</param>
        /// <param name="testExecutionService">Service to access test execution data.</param>
        /// <param name="environmentService">Service to access environment data.</param>
        /// <param name="testLevelService">Service to access test level data.</param>
        /// <param name="importTestResultService">Service that performs import of test results.</param>
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

        /// <summary>
        /// Bound property for the current test execution identifier.
        /// Populated from route or form on GET/POST.
        /// </summary>
        [BindProperty]
        public long ExecId { get; set; } = 0;

        /// <summary>
        /// Bound property for the specific test execution item identifier associated with the execution.
        /// </summary>
        [BindProperty]
        public long ExecItemId { get; set; } = 0;

        /// <summary>
        /// Files uploaded by the user for import. Bound from the form file input.
        /// </summary>
        [BindProperty]
        public List<IFormFile> UploadFiles { get; set; } = new List<IFormFile>();

        /// <summary>
        /// Collection of available test levels to present in the UI for selection.
        /// </summary>
        public ICollection<TestLevelViewModel> TestLevels { get; set; } = new List<TestLevelViewModel>();

        /// <summary>
        /// Currently selected test level identifier. Nullable when no selection has been made.
        /// Bound from the form submission.
        /// </summary>
        [BindProperty]
        public long? SelectedTestLevelId { get; set; } = 0;

        /// <summary>
        /// View model representing the test execution being displayed on the page.
        /// </summary>
        public ExecutionViewModel ExecutionViewModel { get; set; } = new ExecutionViewModel();

        /// <summary>
        /// View model representing the environment associated with the current test execution.
        /// </summary>
        public EnvironmentViewModel EnvironmentViewModel { get; set; } = new EnvironmentViewModel();

        /// <summary>
        /// Handles GET requests for the execution upload page. Loads execution details,
        /// the associated environment and the available test levels.
        /// </summary>
        /// <param name="id">Identifier of the test execution to load.</param>
        public async Task OnGetAsync(long id)
        {
            ExecId = id;
            ExecutionViewModel? testExecution = await _testExecutionService!.GetTestExecutionByIdAsync(ExecId);
            if (testExecution is not null)
            {
                ExecutionViewModel = testExecution;
            }
            string envName = ExecutionViewModel.Environment;
            ExecItemId = ExecutionViewModel.TestExecutionItemId;
            var env = await _environmentService!.GetLatestEnvironmentByNameAsync(envName);
            if (env is not null)
            {
                EnvironmentViewModel = env;
            }

            TestLevels = await _testLevelService!.GetTestLevelAsync();
        }

        /// <summary>
        /// Handles POST requests for uploading test result files. Validates inputs,
        /// wraps uploaded files into sources, and calls the import service for each file.
        /// </summary>
        /// <param name="id">Optional test execution identifier from route or form.</param>
        /// <param name="selectedTestLevelId">Optional selected test level identifier from form.</param>
        /// <param name="itemId">Optional test execution item identifier from route or form.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the POST action.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadFiles.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select at least one file to upload.");
                return RedirectToPage("./Index", new { id = ExecId });
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
                        return RedirectToPage();
                    }

                    long testLevelId = SelectedTestLevelId ?? 0;
                    await _importTestResultService.ImportAsync(ExecId, ExecItemId, testLevelId, request);
                    _logger?.LogInformation("Imported test results from uploaded file {FileName}", fileItem.FileName);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error importing uploaded file {FileName}", fileItem.FileName);
                    ModelState.AddModelError(string.Empty, $"Failed to import file {fileItem.FileName}: {ex.Message}");

                    return RedirectToPage("./Index", new { id = ExecId });
                }
            }

            TempData["SuccessMessage"] = "Files imported successfully.";
            return RedirectToPage("./Index", new { id = ExecId });
        }
    }
}
