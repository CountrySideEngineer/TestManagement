using System.Net.Http.Headers;
using System.Text.Json;
using TestManagement.APP.Models;

namespace TestManagement.APP.Services
{
    public class DashboardApiClient
    {
        private readonly HttpClient _httpClient;

        public DashboardApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("TestApiClient");
        }

        public async Task<SummaryDto> GetSummaryAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Models.TestRunDto>>("api/testrun/");
                var latestRun = response?.OrderByDescending(_ => _.ExecutedAt).First().Id;
                var testResults = await _httpClient.GetFromJsonAsync<List<Models.TestResultDto>>($"api/testrun/");
                var testRunResults = testResults?.Where(_ => _.TestRunId == latestRun).ToList();
                int execNum = null == testResults ? 0 : testResults.Count();
                int errNum = null == testResults ? 0 : testResults.Where(_ => _.Status == TestStatus.Failure).Count();
                var summary = new SummaryDto(errNum, execNum);

                return summary;
            }
            catch (Exception)
            {
                return new SummaryDto(0, 0);
            }
        }

        public async Task<List<Models.TestResultDto>> GetTestRecordsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Models.TestRunDto>>("api/testrun/");
                var latestRun = response?.OrderByDescending(_ => _.ExecutedAt).First().Id;
                var testResults = await _httpClient.GetFromJsonAsync<List<Models.TestResultDto>>($"api/testresult/");
                var testRunResults = testResults?.Where(_ => _.TestRunId == latestRun).ToList();

                return (null == testRunResults ? new List<Models.TestResultDto>() : testRunResults);
            }
            catch (Exception)
            {
                return new List<TestResultDto>();
            }
        }


        public async Task<Models.DashboardDto> GetDashboardAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Models.TestRunDto>>("api/testrun");
            try
            {
                //テスト実行結果の取得

                var latestRun = response?.OrderByDescending(_ => _.ExecutedAt).First().Id;
                var testResults = await _httpClient.GetFromJsonAsync<List<Models.TestResultDto>>($"api/testrun/");
                var testRunResults = testResults?.Where(_ => _.TestRunId == latestRun).ToList();

                int execNum = null == testResults ? 0 : testResults.Count();
                int errNum = null == testResults ? 0 : testResults.Where(_ => _.Status == TestStatus.Failure).Count();
                var summary = new SummaryDto(errNum, execNum);



            }
            catch (Exception ex)
            {
            }

            return new Models.DashboardDto();
        }

        public async Task UploadTestResultAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (file.Length == 0) throw new ArgumentException("Empty file.", nameof(file));

            // 1) 新しい TestRun を作成（API が作成したオブジェクトを返す前提）
            var newTestRun = new Models.TestRunDto
            {
                ExecutedAt = DateTime.UtcNow,
                Environment = "Automated Upload",
                Notes = "Uploaded via DashboardApiClient"
            };

            var runResponse = await _httpClient.PostAsJsonAsync("api/testrun", newTestRun, cancellationToken);
            runResponse.EnsureSuccessStatusCode();

            // API が作成した TestRun を返すことを期待（Id を取得）
            var createdRun = await runResponse.Content.ReadFromJsonAsync<Models.TestRunDto>(cancellationToken: cancellationToken)
                             ?? throw new InvalidOperationException("Created TestRun was not returned.");

            // 2) ファイルを multipart/form-data でアップロード（例: api/testresult/upload/{runId}）
            using var stream = file.OpenReadStream();
            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(stream);
            if (!string.IsNullOrWhiteSpace(file.ContentType))
            {
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            }

            content.Add(streamContent, "file", Path.GetFileName(file.FileName));
            // 必要に応じてメタデータを追加
            content.Add(new StringContent(createdRun.Id.ToString()), "testRunId");

            using FileStream fileStream = new FileStream(file.Name, FileMode.OpenOrCreate);
            {
                await file.CopyToAsync(fileStream);
            }

        }
    }
}
