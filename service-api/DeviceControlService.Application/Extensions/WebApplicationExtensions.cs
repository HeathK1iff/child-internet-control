using DeviceControlService.Application.Middlewares;

public static class WebApplicationExtensions
{
    public static WebApplication UseHeaderRestriction(this WebApplication app)
    {
        app.UseMiddleware<HeaderRestrictionMiddleware>();
        
        return app;
    }
}