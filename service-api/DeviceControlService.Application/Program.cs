using System.Reflection;
using DeviceControlService.Application.MapperProfile;
using DeviceControlService.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSerilog();
builder.Services.AddAutoMapper(cf =>
{
    cf.AddMaps([Assembly.GetAssembly(typeof(DeviceMapperProfile))]);
});

const string CorsPolicy = "default";
const string DefaultOrigin = "http://localhost:8081";

string corsOrigin = builder.Configuration["Cors:Origins"] ?? DefaultOrigin;

builder.Services.AddCors(a =>
{
    a.AddPolicy(CorsPolicy, policy =>
    {
        policy.WithOrigins(corsOrigin)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


builder.Services.AddControllers();
builder.Services.AddKeeneticHttpClient(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors(CorsPolicy);
app.UseIPAddressRestriction();

app.MapControllers();

app.Run();

