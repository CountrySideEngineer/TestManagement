using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

// Web API を呼ぶための HttpClient を作る
var client = new HttpClient();
// POST メソッドで JSON の Body のリクエストを投げる
var postResponse = await client.PostAsJsonAsync(
    @"https://localhost:7162/api/TestRun",
    new TestRunModel
    {
        ExecutedAt = DateTime.UtcNow,
        Environment = "Linux",
        Notes = "Sample test run from C# client application"
    });
if (postResponse.IsSuccessStatusCode)
{
    Console.WriteLine("POST succeeded.");

    var item = await postResponse.Content.ReadFromJsonAsync<TestRunModel>();
    Console.WriteLine($"Id: {item.Id}," +
        $"Notes: {item.Notes}, " +
        $"Environment: {item.Environment}, " +
        $"Executed: {item.ExecutedAt.ToString("yyyy-MM-dd, HH:mm:ss")}"
    );

}
else
{
    Console.WriteLine("POST failed.");
}

var response = await client.GetAsync(
    @"https://localhost:7162/api/TestRun");
// レスポンスのステータスコードが成功していたら Answer の値を出力
if (response.IsSuccessStatusCode)
{
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(content);

    var item = await response.Content.ReadFromJsonAsync<List<TestRunModel>>();
    Console.WriteLine(item);

    foreach (var record in item)
    {
        Console.WriteLine($"Id: {record.Id}," +
            $"Notes: {record.Notes}, " +
            $"Environment: {record.Environment}, " +
            $"Executed: {record.ExecutedAt.ToString("yyyy-MM-dd, HH:mm:ss")}"
        );
    }

    // Get latest
    var latest = item.OrderByDescending(_ => _.CreatedAt).FirstOrDefault()!;
    Console.WriteLine($"Id: {latest.Id}," +
        $"Notes: {latest.Notes}, " +
        $"Environment: {latest.Environment}, " +
        $"Executed: {latest.ExecutedAt.ToString("yyyy-MM-dd, HH:mm:ss")}",
        $"CreatedAt: {latest.CreatedAt.ToString("yyyy-MM-dd, HH:mm:ss")}"
    );
}

public class TestLevelModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class TestCaseModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int TestLevelId { get; set; }
    public TestLevelModel TestLevel { get; set; } = new TestLevelModel();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class TestResultModel
{
    public int Id { get; set; }

    public string ActualResult { get; set; } = string.Empty;

    public int TestCaseId { get; set; }
    public TestCaseModel TestCase { get; set; } = new TestCaseModel();

    public int TestRunId { get; set; }
    public TestRunModel TestRun { get; set; } = new TestRunModel();


    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum TestStatus
{
    Unknown = 0,
    Success = 1,
    Failure = 2,
    Skipped = 3,
    Blocked = 4
}

public class TestRunModel
{
    public int Id { get; set; }

    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

    public string Environment { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TestResultModel> TestResults { get; set; } = new List<TestResultModel>();
}
