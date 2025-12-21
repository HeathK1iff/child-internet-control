using System.Reflection;
using DeviceControlService.Application.MapperProfile;
using DeviceControlService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddCors(a =>
{
    a.AddDefaultPolicy(p =>
    {
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod();
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
app.UseCors();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

