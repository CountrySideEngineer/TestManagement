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

namespace TestManagement.APP.Pages.Upload
{
    public class IndexModel : PageModel
    {
        private readonly IRequestRepository _repository;

        private readonly TestLevelApiClient _testLevelApiClient;

        private readonly TestRunApiClient _testRunApiClient;

        private readonly UploadFileParser _uploadFileParser;

        public IndexModel(IRequestRepository repository, TestLevelApiClient apiClient, TestRunApiClient testRunApi, UploadFileParser uploadFileParser)
        {
            _repository = repository;
            _testLevelApiClient = apiClient;
            _testRunApiClient = testRunApi;
            _uploadFileParser = uploadFileParser;
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

            // 解析処理: UploadFiles を TestResultDto のコレクションに変換する。
            IList<TestResultDto> parsedResults = await _uploadFileParser.ParseAsync(UploadFiles, SelectedTestLevelId.Value);

            // 実行情報の割当: 新規作成 or 既存の選択
            int runIdForResults = 0;
            if (ExecutionMode == "new")
            {
                // 新規要求を作成して、その ID を利用する。
                // ただし RequestRepository.AddAsync は Request を DB に保存し ID を割り当てる。
                // ここでは一旦ディレクトリを作成して Request を登録する処理の後で RunId をセットするためフラグを残す。
            }
            else
            {
                // 既存の実行情報を指定している場合は、その ID を TestResultDto に設定する
                runIdForResults = SelectedExecutionInfoId.Value;
            }

            // 指定されたファイルを格納するディレクトリを作成する。
            // ディレクトリ名は、タイムスタンプで一意に決定する。
            DateTime timeStamp = DateTime.UtcNow;
            string driveRoot = Path.GetPathRoot(Directory.GetCurrentDirectory())!;
            string uploadPath = Path.Combine(driveRoot, "Uploads", timeStamp.ToString("yyyyMMdd_HHmmssfff"));
            Directory.CreateDirectory(uploadPath);

            // 作成したディレクトリに、ファイルをアップロードする。
            foreach (var fileItem in UploadFiles)
            {
                using var stream = fileItem.OpenReadStream();
                using var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(stream);
                content.Add(streamContent, "file", fileItem.FileName);

                string filePath = Path.Combine(uploadPath, fileItem.FileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    await fileItem.CopyToAsync(fileStream);
                }
            }

            // 新規要求を発行する。
            var newRequest = new Request()
            {
                DirectoryPath = uploadPath,
                StatusId = 1,
                ResultId = 1,
                TestLevelId = (int)SelectedTestLevelId
            };
            await _repository.AddAsync(newRequest);

            // 新しい Request がデータベース上で作成され、ID が付与されているはずなので
            // ExecutionMode が "new" の場合は、ここで新規作成された Request の Id を RunId として利用する。
            if (ExecutionMode == "new")
            {
                runIdForResults = newRequest.Id;
            }

            // parsedResults の各要素に RunId を設定する
            foreach (var r in parsedResults)
            {
                r.TestRunId = runIdForResults;
            }

            // TODO: parsedResults を別のサービスへ送信するなどの処理をここに追加

            //TempData["UploadMessage"] = $"{started} 件のアップロードを開始しました。処理状況はダッシュボードで確認してください。";
            return RedirectToPage("/Dashboard");
        }
    }
}

