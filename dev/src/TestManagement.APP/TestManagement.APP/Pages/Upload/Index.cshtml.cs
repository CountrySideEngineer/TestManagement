using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using TestManagement.APP.Data;
using TestManagement.APP.Data.Repositories.TestAnalysis;
using TestManagement.APP.Models.TestAnalysis;
using TestManagement.APP.Services;
using TestManagement.APP.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TestManagement.APP.Services.Option;

namespace TestManagement.APP.Pages.Upload
{
    public class IndexModel : PageModel
    {
        private readonly IRequestRepository _repository;

        private readonly TestLevelApiClient _testLevelApiClient;

        private readonly TestRunApiClient _testRunApiClient;

        private readonly TestCaseApiClient _testCaseApiClient;

        private readonly UploadFileParser _uploadFileParser;

        private readonly UploadApiClient _uploadApiClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="apiClient"></param>
        /// <param name="testRunApi"></param>
        /// <param name="uploadFileParser"></param>
        /// <param name="testCaseApiClient"></param>
        public IndexModel(
            IRequestRepository repository, 
            TestLevelApiClient apiClient, 
            TestRunApiClient testRunApi, 
            UploadFileParser uploadFileParser, 
            TestCaseApiClient testCaseApiClient,
            UploadApiClient uploadApiClient
            )
        {
            _repository = repository;
            _testLevelApiClient = apiClient;
            _testRunApiClient = testRunApi;
            _uploadFileParser = uploadFileParser;
            _testCaseApiClient = testCaseApiClient;
            _uploadApiClient = uploadApiClient;
        }

        [BindProperty]
        public List<IFormFile> UploadFiles { get; set; } = new List<IFormFile>();

        // 画面に表示するテストレベル一覧
        public List<TestLevelDto> TestLevels { get; set; } = new List<TestLevelDto>();

        // 実行情報一覧（UI のドロップダウンで使用）
        public IList<TestRunDto> ExecutionInfos { get; set; } = new List<TestRunDto>();

        // ドロップダウンの選択値（必要なら POST 時に利用可能）
        [BindProperty]
        public int? SelectedTestLevelId { get; set; }

        // ドロップダウン選択: 実行情報のId
        [BindProperty]
        public int? SelectedExecutionInfoId { get; set; }

        // ラジオで選択されたモード（"new" または "existing"）を受け取る
        [BindProperty]
        public string ExecutionMode { get; set; } = "new";

        [BindProperty]
        public string NewRevision { get; set; } = string.Empty;

        [BindProperty]
        public DateTime NewRevisionDate { get; set; } = DateTime.UtcNow;

        public async Task OnGetAsync()
        {
            try
            {
                TestLevels = await _testLevelApiClient.GetTestLevelsAsync() ?? new List<TestLevelDto>();
            }
            catch (Exception)
            {
                // 取得失敗時は空リストにする（ログを追加する場合はここで）
                TestLevels = new List<TestLevelDto>();
            }

            try
            {
                ExecutionInfos = await _testRunApiClient.GetTestRunsAsync() ?? new List<TestRunDto>();
            }
            catch (Exception)
            {
                ExecutionInfos = new List<TestRunDto>();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadFiles == null || UploadFiles.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "ファイルを選択してください。");
                return Page();
            }
            if (null == SelectedTestLevelId)
            {
                ModelState.AddModelError(string.Empty, "テストレベルを選択してください。");
                return Page();
            }

            var newTestRun = new TestRunDto()
            {
                Abstract = NewRevision,
                ExecutedAt = NewRevisionDate.ToUniversalTime()
            };
            TestRunDto regTestRun = await _uploadApiClient.CreateTestRunAsync(newTestRun);
            ParseOption parseOption = new ParseOption()
            {
                TestLevelId = SelectedTestLevelId,
                RevisionId = regTestRun.Id
            };
            IList<TestResultDto> testResults = await _uploadFileParser.ParseAsync(UploadFiles, parseOption);
            List<TestCaseDto> testCases = testResults.Select(r => r.TestCase!).ToList();
            await _uploadApiClient.CreateTestCaseAsync(testCases);
            await _uploadApiClient.ApplyTestCaseAsync(testCases);
            foreach (var testResult in testResults)
            {
                if ((null != testResult.TestCase) && (0 != testResult.TestCase.Id))
                {
                    testResult.TestCaseId = testResult.TestCase.Id;
                }
            }
            await _uploadApiClient.CreateTestResultAsync(testResults);

            return RedirectToPage("/index");
        }
    }
}

