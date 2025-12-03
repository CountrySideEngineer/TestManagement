using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
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
    public List<TestRecordDto> TestRecords { get; set; } = [];
    public List<RequestTrendDto> RequestTrend { get; set; } = [];
    public List<ErrorDto> Errors { get; set; } = [];

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


    public async Task OnGet()
    {
        await _apiClient.GetDashboardAsync();
    }
}

public record SummaryDto(int ErrorNum, int ExecutedNum);
public record TestRecordDto(DateTime ExecutedAt, string Result);
public record RequestTrendDto(string Time, int Count);
public record ErrorDto(string Time, string Message);
