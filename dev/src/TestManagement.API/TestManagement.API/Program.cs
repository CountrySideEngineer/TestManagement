using Microsoft.EntityFrameworkCore;
using System;
using TestManagement.API.Data;
using TestManagement.API.Data.Repositories;
using TestManagement.API.Infrastructure.Database;
using TestManagement.API.Services;
using TestManagement.Data.Repositories;
using TestManagement.API.Services.Xml;

var builder = WebApplication.CreateBuilder(args);

string connectionString = DBConnectionFactory.CreatePostgresConnectionString(builder.Configuration);

// Add services to the container.
builder.Services.AddDbContext<TestManagementDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITestLevelRepository, TestLevelRepository>();
builder.Services.AddScoped<TestLevelService>();
builder.Services.AddScoped<ITestCaseRepository, TestCaseRepository>();
builder.Services.AddScoped<TestCaseService>();

// XML converter
builder.Services.AddScoped<ITestResultXmlConverter, TestResultXmlConverter>();

builder.Services.AddScoped<ITestResultRepository, TestResultRepository>();
builder.Services.AddScoped<TestResultService>();
builder.Services.AddScoped<ITestRunRepository, TestRunRepository>();
builder.Services.AddScoped<TestRunService>();
builder.Services.AddMvc().AddXmlSerializerFormatters();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
