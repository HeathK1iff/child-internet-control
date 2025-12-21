using System.Reflection;
using DeviceControlService.Application.MapperProfile;
using DeviceControlService.Application.Middlewares;
using DeviceControlService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddLogging(c => c.AddConsole());
builder.Services.AddAutoMapper(cf =>
{
    cf.AddMaps([Assembly.GetAssembly(typeof(DeviceMapperProfile))]);
});

const string CorsPolicy = "all";
builder.Services.AddCors(a =>
{
    a.AddPolicy(CorsPolicy, p =>
    {
        p.AllowAnyOrigin();
        p.AllowAnyMethod();
        p.AllowAnyHeader();
    });
});



builder.Services.AddControllers();
builder.Services.AddKeeneticHttpClient(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors(CorsPolicy);
app.MapControllers();
app.UseMiddleware<HeaderRestrictionMiddleware>();
app.Run();

