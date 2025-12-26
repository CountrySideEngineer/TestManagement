using System.Net.Http.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TestManagement.APP.Models;
using TestManagement.APP.Services;
using System.Threading.Tasks;
using System.Threading;

public class DashboardModel : PageModel
{
    private readonly DashboardApiClient _apiClient;

    public DashboardModel(DashboardApiClient apiClient)
    {
        _apiClient = apiClient;

        Summary = new SummaryDto(0, 0, 0, 0);
    }

    public SummaryDto? Summary { get; set; }
    public TestRunDto TestRun { get; set; } = new TestRunDto();
    public List<TestResultDto> TestResults { get; set; } = new List<TestResultDto>();
    public List<TestRecordDto> TestRecords { get; set; } = new List<TestRecordDto>();
    public List<RequestTrendDto> RequestTrend { get; set; } = new List<RequestTrendDto>();
    public List<ErrorDto> Errors { get; set; } = new List<ErrorDto>();

    [BindProperty]
    public List<IFormFile> UploadFiles { get; set; } = new List<IFormFile>();

    // Chart.js 用 JSON
    public string RequestTrendLabelsJson => System.Text.Json.JsonSerializer.Serialize(RequestTrend.Select(x => x.Time));
    public string RequestTrendValuesJson => System.Text.Json.JsonSerializer.Serialize(RequestTrend.Select(x => x.Count));

    public string GetErrorRatePercent()
        => Summary == null ? "--" : (Summary.ErrorNum * 100).ToString("F0") + "%";

    public string GetPassRatePercent()
    {
        if ((Summary == null) || (0 == Summary.ExecutedNum))
        {
            return "--";
        }
        else
        {
            int passedRate = (((Summary.ExecutedNum - Summary.ErrorNum) * 100) / Summary.ExecutedNum);
            string passedRateInStr = passedRate.ToString("F0") + "%";
            return passedRateInStr;
        }
    }

    public async Task OnGetAsync()
    {
        Summary = await _apiClient.GetSummaryAsync();
        TestRun = await _apiClient.GetLatestTestRunAsync();
        TestResults = await _apiClient.GetTestRecordsByTestRunAsync(TestRun.Id);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (UploadFiles == null || UploadFiles.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "ファイルを選択してください。");
            return Page();
        }

        var tasks = new List<Task>();
        foreach (var uploadFile in UploadFiles)
        {
            if (uploadFile == null || uploadFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, $"空のファイルは無視されました: {uploadFile?.FileName}");
                continue;
            }

            // ファイルサイズや ContentType のチェック（例: 10MB 上限）
            const long maxSize = 10 * 1024 * 1024;
            if (uploadFile.Length > maxSize)
            {
                ModelState.AddModelError(string.Empty, $"ファイルが大きすぎます: {uploadFile.FileName}");
                continue;
            }

            // 非同期メソッドを待機するため Task を収集
            tasks.Add(_apiClient.UploadTestResultAsync(uploadFile, cancellationToken));
        }

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            // ログや ModelState への追加
            ModelState.AddModelError(string.Empty, $"アップロード中にエラーが発生しました: {ex.Message}");
            return Page();
        }

        return RedirectToPage();
    }
}

public record SummaryDto(int ErrorNum, int SkippedNum, int DisabledNum, int ExecutedNum);
public record TestRecordDto(DateTime ExecutedAt, string Result);
public record RequestTrendDto(string Time, int Count);
public record ErrorDto(string Time, string Message);
