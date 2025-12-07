using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using TestManagement.APP.Data;
using TestManagement.APP.Data.Repositories.TestAnalysis;
using TestManagement.APP.Models.TestAnalysis;

public class UploadModel : PageModel
{
    private readonly IRequestRepository _repository;

    public UploadModel(IRequestRepository repository)
    {
        _repository = repository;
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

        // 指定されたファイルを格納するディレクトリを作成する。
        // ディレクトリ名は、タイムスタンプで一意に決定する。
        DateTime timeStamp = DateTime.UtcNow;
        string dirName = timeStamp.ToString("yyyyMMdd_HHmmssfff");
        string dirPath = Path.Combine("Uploads", dirName);
        Directory.CreateDirectory(dirPath);

        // 作成したディレクトリに、ファイルをアップロードする。
        foreach (var fileItem in UploadFiles)
        {
            using var stream = fileItem.OpenReadStream();
            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(stream);
            content.Add(streamContent, "file", fileItem.FileName);

            string filePath = Path.Combine(dirPath, fileItem.FileName);
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                await fileItem.CopyToAsync(fileStream);
            };
        }

        // 新規要求を発行する。
        var newRequest = new Request()
        {
            DirectoryPath = dirPath,
            StatusId = 1,
            ResultId = 1
        };
        await _repository.AddAsync(newRequest);

        //TempData["UploadMessage"] = $"{started} 件のアップロードを開始しました。処理状況はダッシュボードで確認してください。";
        return RedirectToPage("/Dashboard");
    }
}