using System.Reflection;
using DeviceControlService.Application.MapperProfile;
using DeviceControlService.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
const string DefaultCorsPolicyName = "default";

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSerilog();
builder.Services.AddAutoMapper(cf =>
{
    cf.AddMaps([Assembly.GetAssembly(typeof(DeviceMapperProfile))]);
});

builder.Services.AddCors(a =>
{
    a.AddPolicy(DefaultCorsPolicyName, policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});


builder.Services.AddControllers();
builder.Services.AddKeeneticHttpClient(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapControllers();
app.UseCors(DefaultCorsPolicyName);
app.UseHostFiltering();
app.UseHeaderRestriction();
app.UseHttpsRedirection();

app.Run();

