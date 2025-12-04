using System.Net.Http.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestManagement.APP.Models;
using TestManagement.APP.Services;

public class DashboardModel : PageModel
{
    private readonly DashboardApiClient _apiClient;

    public DashboardModel(DashboardApiClient apiClient)
    {
        _apiClient = apiClient;

        Summary = new SummaryDto(0, 0);
    }

    public SummaryDto? Summary { get; set; }
    public List<TestResultDto> TestResults { get; set; } = new List<TestResultDto>();
    public List<TestRecordDto> TestRecords { get; set; } = new List<TestRecordDto>();
    public List<RequestTrendDto> RequestTrend { get; set; } = new List<RequestTrendDto>();
    public List<ErrorDto> Errors { get; set; } = new List<ErrorDto>();

    // Chart.js —p JSON
    public string RequestTrendLabelsJson => System.Text.Json.JsonSerializer.Serialize(RequestTrend.Select(x => x.Time));
    public string RequestTrendValuesJson => System.Text.Json.JsonSerializer.Serialize(RequestTrend.Select(x => x.Count));

    public string GetErrorRatePercent()
        => Summary == null ? "--" : (Summary.ErrorNum * 100).ToString("F2") + "%";

    public string GetPassRatePercent()
    {
        if ((Summary == null) || (0 == Summary.ExecutedNum))
        {
            return "--";
        }
        else
        {
            return ((1 - Summary.ErrorNum) * 100).ToString("F2") + "%";
        }
    }


    public async Task OnGetAsync()
    {
        Summary = await _apiClient.GetSummaryAsync();
        TestResults = await _apiClient.GetTestRecordsAsync();
    }
}

public record SummaryDto(int ErrorNum, int ExecutedNum);
public record TestRecordDto(DateTime ExecutedAt, string Result);
public record RequestTrendDto(string Time, int Count);
public record ErrorDto(string Time, string Message);
