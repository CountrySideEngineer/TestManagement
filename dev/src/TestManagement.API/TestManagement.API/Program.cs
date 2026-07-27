using Microsoft.EntityFrameworkCore;
using System;
using TestManagement.API.Data;
using TestManagement.API.Infrastructure.Configuration;
using TestManagement.API.Infrastructure.Database;
using TestManagement.API.Infrastructure.IO;
using TestManagement.API.Services;
using TestManagement.API.Services.Xml;

var builder = WebApplication.CreateBuilder(args);

var fileReader = new FileReader();
var configUtility = new ConfigUtility(builder.Configuration);
var dbConnection = new DBConnectionFactory(configUtility, fileReader);
string connectionString = dbConnection.CreatePostgresConnectionString();

builder.Services.AddDbContext<TestManagementDbContext>(options => options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddScoped<ITestLevelService, TestLevelService>();
builder.Services.AddScoped<ITestCaseService, TestCaseService>();
builder.Services.AddScoped<ITestExecutionService, TestExecutionService>();
builder.Services.AddScoped<IEnvironmentService, EnvironmentService>();
builder.Services.AddScoped<ITestResultService, TestResultService>();

// XML converter
builder.Services.AddScoped<ITestResultXmlConverter, TestResultXmlConverter>();

builder.Services.AddMvc().AddXmlSerializerFormatters();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
