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

Console.WriteLine($"{nameof(apiBaseUrl)} = {apiBaseUrl}");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<TestLevelApiClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
