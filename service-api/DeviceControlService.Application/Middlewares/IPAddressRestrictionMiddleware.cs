namespace DeviceControlService.Application.Middlewares;

public sealed class IPAddressRestrictionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _allowedIps;
    private readonly ILogger<IPAddressRestrictionMiddleware> _logger;

    public IPAddressRestrictionMiddleware(RequestDelegate next, 
        IConfiguration configuration, ILogger<IPAddressRestrictionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        var configValue = configuration["AllowedAccessIPs"] ?? string.Empty;
        
        _logger.LogInformation($"Following IPs are accessed: {configValue}");

        _allowedIps = configValue.Split(';').ToHashSet();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string ipAddress = context.Connection?.RemoteIpAddress?.MapToIPv4()?.ToString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            await _next(context);
        }

        if ((_allowedIps is {Count: > 0}) && (!_allowedIps.Contains(ipAddress)))
        {
            _logger.LogWarning($"Request restricted by IP: {ipAddress}");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access restricted by IP!!!");
            return;
        }
        
        await _next(context);
    }
}