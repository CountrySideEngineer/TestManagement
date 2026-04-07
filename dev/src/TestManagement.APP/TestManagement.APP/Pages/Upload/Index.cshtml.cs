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
using TestManagement.APP.Dto.TestExecution.Get;

namespace TestManagement.APP.Pages.Upload
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel>? _logger;

        private readonly ITestExecutionService? _testExecutionService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uploadFileParser">File parser object.</param>
        /// <param name="uploadApiClient">Test result upload API client.</param>
        public IndexModel(
            ILogger<IndexModel>? logger,
            ITestExecutionService? testExecutionService
            )
        {
            _logger = logger;
            _testExecutionService = testExecutionService;
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

        public ICollection<GetTestExecutionResponse>? TestExecutions { get; set; }

        public async Task OnGetAsync()
        {
            TestExecutions = await _testExecutionService!.GetTestExecutionsAsync();
        }
    }
}

