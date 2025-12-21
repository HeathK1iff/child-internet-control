using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DeviceControlService.Application.Responses;
using DeviceControlService.Domain.Abstractions;
using DeviceControlService.Application.Dto;

namespace DeviceControlService.Application.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class DevicesController(IActivateInternetDeviceService deviceService, IMapper mapper): ControllerBase
{
    readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    readonly IActivateInternetDeviceService _deviceService = deviceService ?? throw new ArgumentNullException(nameof(deviceService)); 

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<DeviceDto>>> GetAll(CancellationToken cancellationToken)
    {
        var ret = await _deviceService.GetClientsAsync(cancellationToken);
        return new ApiResponse<IEnumerable<DeviceDto>>(
            Message: string.Empty, 
            Payload: _mapper.Map<IEnumerable<DeviceDto>>(ret));
    }

    [HttpGet("activate")]
    public async Task<ApiResponse<object>> ActivateInternet([FromQuery] string mac, CancellationToken cancellationToken)
    {
        var ret = await _deviceService.ActivateInternetForClientAsync(mac, true, cancellationToken);
        return new ApiResponse<object>("Success", null);
    }

    [HttpGet("deactivate")]
    public async Task<ApiResponse<object>> DeactivateInternet([FromQuery] string mac, CancellationToken cancellationToken)
    {
        var ret = await _deviceService.ActivateInternetForClientAsync(mac, false, cancellationToken);
        return new ApiResponse<object>("Success", null);
    }
    
}
