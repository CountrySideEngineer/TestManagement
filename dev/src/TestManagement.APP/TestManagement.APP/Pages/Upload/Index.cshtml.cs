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

namespace TestManagement.APP.Pages.Upload
{
    public class IndexModel : PageModel
    {
        private readonly IRequestRepository _repository;

        private readonly TestLevelApiClient _apiClient;

        public IndexModel(IRequestRepository repository, TestLevelApiClient apiClient)
        {
            _repository = repository;
            _apiClient = apiClient;
        }

        [BindProperty]
        public List<IFormFile> UploadFiles { get; set; } = new List<IFormFile>();

        // 画面に表示するテストレベル一覧
        public List<TestLevelDto> TestLevels { get; set; } = new List<TestLevelDto>();

        // ドロップダウンの選択値（必要なら POST 時に利用可能）
        [BindProperty]
        public int? SelectedTestLevelId { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                TestLevels = await _apiClient.GetTestLevelsAsync() ?? new List<TestLevelDto>();
            }
            catch (Exception)
            {
                // 取得失敗時は空リストにする（ログを追加する場合はここで）
                TestLevels = new List<TestLevelDto>();
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
                ;
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

            //TempData["UploadMessage"] = $"{started} 件のアップロードを開始しました。処理状況はダッシュボードで確認してください。";
            return RedirectToPage("/Dashboard");
        }
    }
}

