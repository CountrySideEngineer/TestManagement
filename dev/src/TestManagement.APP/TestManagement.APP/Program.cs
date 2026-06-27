using Microsoft.EntityFrameworkCore;
using TestManagement.APP.ApiClients;
using TestManagement.APP.ApiClients.Environment;
using TestManagement.APP.ApiClients.TestCase;
using TestManagement.APP.ApiClients.TestLevel;
using TestManagement.APP.ApiClients.TestResult;
using TestManagement.APP.Services;
using TestManagement.APP.Services.Environment;
using TestManagement.APP.Services.TestCase.Sync;
using TestManagement.APP.Services.TestExecution;
using TestManagement.APP.Services.TestExecution.Import;
using TestManagement.APP.Services.TestExecution.Register;
using TestManagement.APP.Services.TestLevel;

var builder = WebApplication.CreateBuilder(args);

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

builder.Services.AddHttpClient("TestApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl ?? throw new InvalidOperationException("API base URL is not configured."));
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

// Add services to the container.
//builder.Services.AddRazorPages();
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/", "");
    });
builder.Services.AddScoped<UploadFileParser>();
builder.Services.AddScoped<ITestExecutionApiClient, TestExecutionApiClient>();
builder.Services.AddScoped<IEnvironmentApiClient, EnvironmentApiClient>();
builder.Services.AddScoped<ITestExecutionService, TestExecutionService>();
builder.Services.AddScoped<IEnvironmentService, EnvironmentService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ITestLevelService, TestLevelService>();
builder.Services.AddScoped<ITestLevelApiClient, TestLevelApiClient>();
builder.Services.AddScoped<ITestCaseSyncApiClient, TestCaseSyncApiClient>();
builder.Services.AddScoped<ITestResultApiClient, TestResultApiClient>();
builder.Services.AddScoped<IRegisterTestExecutionService, RegisterTestExecutionService>();
builder.Services.AddScoped<IImportTestResultService, ImportTestResultService>();
builder.Services.AddScoped<ISyncTestCasesService, SyncTestCasesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Redirect HTTP to HTTPS. Ensure this middleware runs early.
app.UseHttpsRedirection();

// Serve static files
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
