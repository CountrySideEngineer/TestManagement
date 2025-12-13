using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

// Web API を呼ぶための HttpClient を作る
var client = new HttpClient();
var response = await client.GetAsync(
    @"https://localhost:7162/api/TestLevel");
// レスポンスのステータスコードが成功していたら Answer の値を出力
if (response.IsSuccessStatusCode)
{
    var item = await response.Content.ReadFromJsonAsync<List<TestLevelModel>>();
    Console.WriteLine(item);


    if (null == item)
    {
        return;
    }
    foreach (var record in item)
    {
        Console.WriteLine($"TestLevel Id: {record.Id}," +
            $"Title: {record.Name}, " +
            $"Description: {record.Description}");
    }
}

class TestLevelModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
