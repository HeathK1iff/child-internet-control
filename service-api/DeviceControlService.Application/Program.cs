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

builder.Services.AddCors(a =>
{
    a.AddPolicy("all", p =>
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

app.MapControllers();
app.UseCors("all");
app.UseMiddleware<HeaderRestrictionMiddleware>();
app.UseHttpsRedirection();

app.Run();

