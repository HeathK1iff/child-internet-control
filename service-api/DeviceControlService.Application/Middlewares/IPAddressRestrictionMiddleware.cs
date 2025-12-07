namespace DeviceControlService.Application.Middlewares;

public sealed class IPAddressRestrictionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _allowedIps;

    public IPAddressRestrictionMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        var configValue = configuration["AllowedAccessIPs"] ?? string.Empty;
        _allowedIps = configValue.Split(';').ToHashSet();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string ipAddress = context.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            await _next(context);
        }

        if (!_allowedIps.Contains(ipAddress))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access restricted by IP!!!");
            return;
        }
        await _next(context);
    }
}