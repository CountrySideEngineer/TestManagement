using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

public class DashboardModel : PageModel
{
    private readonly HttpClient _http;

    public DashboardModel(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("DashboardApi");
    }

    public SummaryDto? Summary { get; set; }
    public List<RequestTrendDto> RequestTrend { get; set; } = [];
    public List<ErrorDto> Errors { get; set; } = [];

    // Chart.js —p JSON
    public string RequestTrendLabelsJson => System.Text.Json.JsonSerializer.Serialize(RequestTrend.Select(x => x.Time));
    public string RequestTrendValuesJson => System.Text.Json.JsonSerializer.Serialize(RequestTrend.Select(x => x.Count));

    public string GetErrorRatePercent()
        => Summary == null ? "--" : (Summary.ErrorRate * 100).ToString("F2") + "%";

    public async Task OnGet()
    {
    }
}

public record SummaryDto(int TodayRequests, double ErrorRate, int AvgResponseMs);
public record RequestTrendDto(string Time, int Count);
public record ErrorDto(string Time, string Message);
