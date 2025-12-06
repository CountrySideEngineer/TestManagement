using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using TestManagement.APP.Data;

public class UploadModel : PageModel
{
    private readonly AnalysisRequestDbContext _dbContext;

    public UploadModel(AnalysisRequestDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    [BindProperty]
    public List<IFormFile> UploadFiles { get; set; } = new List<IFormFile>();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (UploadFiles == null || UploadFiles.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "ファイルを選択してください。");
            return Page();
        }

        const long maxSize = 10 * 1024 * 1024; // 10MB 上限（必要に応じて変更）

        int started = 0;
        foreach (var file in UploadFiles)
        {
            if (file == null || file.Length == 0)
            {
                // 無視
                continue;
            }

            if (file.Length > maxSize)
            {
                ModelState.AddModelError(string.Empty, $"ファイルが大きすぎます: {file.FileName}");
                return Page();
            }

            // TODO: _dbContext を使った保存処理や、API 呼び出しをここに実装
            started++;
        }

        TempData["UploadMessage"] = $"{started} 件のアップロードを開始しました。処理状況はダッシュボードで確認してください。";
        return RedirectToPage("/Dashboard");
    }
}