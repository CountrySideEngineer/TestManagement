// Web API を呼ぶための HttpClient を作る
using System.Net.Http.Json;

var client = new HttpClient();

// POST メソッドで JSON の Body のリクエストを投げる
var postResponse = await client.PostAsJsonAsync(
    @"https://localhost:7162/api/TestCase",
    new TestCaseModel
    {
        Title = "Sample test case X001",
        Description = "Sample test case post from application implemented in C#",
        TestLevelId = 1,
        TestLevel = new TestLevelModel
        {
            Id = 1,
            Name = "Sample Level A",
            Description = "Sample Level A description"
        }
    });
if (postResponse.IsSuccessStatusCode)
{
    Console.WriteLine("POST succeeded.");
}
else
{
    Console.WriteLine("POST failed.");
}

var response = await client.GetAsync(
        @"https://localhost:7162/api/TestCase");
// レスポンスのステータスコードが成功していたら Answer の値を出力
if (response.IsSuccessStatusCode)
{
    var item = await response.Content.ReadFromJsonAsync<List<TestCaseModel>>();
    Console.WriteLine(item);

    if (null == item)
    {
        return;
    }   
    foreach (var record in item)
    {
        Console.WriteLine($"TestCase Id: {record.Id}," +
            $"Title: {record.Title}, " +
            $"TestLevel Name: {record.TestLevel.Name}");
    }
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
