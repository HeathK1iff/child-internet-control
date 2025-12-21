using System.Reflection;
using DeviceControlService.Application.MapperProfile;
using DeviceControlService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var corsOrigins = builder.Configuration["Cors:Origins"] ?? string.Empty;

builder.Services.AddCors(a =>
{
    a.AddPolicy("All", p =>
    {
        p.WithOrigins(corsOrigins)
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials();
    });
});

builder.Services.AddLogging(c => c.AddConsole());
builder.Services.AddAutoMapper(cf =>
{
    cf.AddMaps([Assembly.GetAssembly(typeof(DeviceMapperProfile))]);
});

builder.Services.AddControllers();
builder.Services.AddKeeneticHttpClient(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.UseCors("All");

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

