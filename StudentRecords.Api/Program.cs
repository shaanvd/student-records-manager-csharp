using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using StudentRecords.App.Repositories;
using StudentRecords.App.Services;
using StudentRecords.App.Logging;

var builder = WebApplication.CreateBuilder(args);

string dbType = builder.Configuration["DatabaseType"] ?? "Memory";

switch (dbType.ToUpper()) //can choose how to save students in appsettings.json
{
    case "CSV":
        builder.Services.AddSingleton<IStudentRepository>(provider =>
            new CsvStudentRepository(Path.Combine("Data", "students.csv")));
        break;

    case "JSON":
        builder.Services.AddSingleton<IStudentRepository>(provider =>
            new JsonStudentRepository(Path.Combine("Data", "students.json")));
        break;

    default:
        builder.Services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
        break;
}

builder.Services.AddSingleton<StudentRecords.App.Logging.ILogger>(provider => new FileLogger(Path.Combine("Data", "system_logs.txt")));
builder.Services.AddSingleton<StudentService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();