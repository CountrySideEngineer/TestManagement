// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestManagement.Analyze.APP.DBContext;
using TestManagement.Analyze.APP.Entities;
using TestManagement.Analyze.APP.Repository;

// Get connection string from application.settings.json
// And set the connection string to DbContext object.
var config = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<RequestDbContext>(options =>
            options.UseNpgsql(connectionString));

    })
    .Build();

// Get "Not started" requests from database.
RequestDbContext dbContext = config.Services.GetRequiredService<RequestDbContext>();
var requestRepo = new GenericRepository<Request>(dbContext);
var requests = requestRepo.GetAll().Where(_ => _.StatusId == (int)STATUS.NOT_STARTED).ToList();

foreach (var requestItem in requests)
{
    Console.WriteLine($"{nameof(requestItem.Id),24} = {requestItem.Id}");
    Console.WriteLine($"{nameof(requestItem.DirectoryPath),24} = {requestItem.DirectoryPath}");
}

// レコードの「ディレクトリパス」に対応するフォルダに格納されたXMLファイルを取得


// XMLファイルを解析して、テスト結果情報を取得


// テスト結果を、データベースの情報に変換


// テスト実行情報を作成


// テスト結果を登録


