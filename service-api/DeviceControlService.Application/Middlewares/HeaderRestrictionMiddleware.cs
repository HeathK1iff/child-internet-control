namespace DeviceControlService.Application.Middlewares;

//TO DO: Temporary solution, not secure
public sealed class HeaderRestrictionMiddleware
{
    const string AuthHeaderKey = "X-Auth";
    private readonly RequestDelegate _next;
    private readonly string _expectedValue;

    public HeaderRestrictionMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _expectedValue = configuration["Auth:Key"] ?? string.Empty;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (string.IsNullOrWhiteSpace(_expectedValue))
        {
            await _next(context);
        }

        if (!context.Request.Headers.ContainsKey(AuthHeaderKey) || 
            context.Request.Headers[AuthHeaderKey] != _expectedValue)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access denied!!!");
            return;
        }
        await _next(context);
    }
}
