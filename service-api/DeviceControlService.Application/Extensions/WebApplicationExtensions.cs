using DeviceControlService.Application.Middlewares;

public static class WebApplicationExtensions
{
    public static WebApplication UseHeaderRestriction(this WebApplication app)
    {
        app.UseMiddleware<HeaderRestrictionMiddleware>();
        return app;
    }

    public static WebApplication UseIPAddressRestriction(this WebApplication app)
    {
        app.UseMiddleware<IPAddressRestrictionMiddleware>();
        return app;
    }
}