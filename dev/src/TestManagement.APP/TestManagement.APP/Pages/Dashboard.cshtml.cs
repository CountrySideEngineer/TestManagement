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

    public List<TestResultDto> GetTestResultsToDisplay()
    {
        if (TestResults == null)
        {
            return new();
        }

        if (10 < TestResults.Count)
        {
            return TestResults.GetRange(0, 10);
        }
        else
        {
            return TestResults;
        }
    }

    public async Task OnGetAsync()
    {
        Summary = await _apiClient.GetSummaryAsync();
        TestRun = await _apiClient.GetLatestTestRunAsync();
        TestResults = await _apiClient.GetTestRecordsByTestRunAsync(TestRun.Id);
    }
}

public record SummaryDto(int ErrorNum, int SkippedNum, int DisabledNum, int ExecutedNum);
public record TestRecordDto(DateTime ExecutedAt, string Result);
public record RequestTrendDto(string Time, int Count);
public record ErrorDto(string Time, string Message);
