using Microsoft.EntityFrameworkCore;
using TestManagement.APP.Data;
using TestManagement.APP.Data.Repositories.TestAnalysis;
using TestManagement.APP.Services;

var builder = WebApplication.CreateBuilder(args);

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

builder.Services.AddHttpClient("TestApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl ?? throw new InvalidOperationException("API base URL is not configured."));
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddDbContext<AnalysisRequestDbContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
//builder.Services.AddRazorPages();
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/", "");
    });
builder.Services.AddScoped<TestRunApiClient>();
builder.Services.AddScoped<TestCaseApiClient>();
builder.Services.AddScoped<TestLevelApiClient>();
builder.Services.AddScoped<DashboardApiClient>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();

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
