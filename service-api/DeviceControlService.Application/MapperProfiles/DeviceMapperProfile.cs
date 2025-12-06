using AutoMapper;
using DeviceControlService.Application.Dto;
using DeviceControlService.Domain.Entities;

namespace DeviceControlService.Application.MapperProfile;

public sealed class DeviceMapperProfile: Profile
{
    public DeviceMapperProfile()
    {
        CreateMap<Device, DeviceDto>()
            .ForMember(x => x.Name, s => s.MapFrom(a => a.Name))
            .ForMember(x => x.MacAddress, s => s.MapFrom(m=>m.MacAddress))
            .ForMember(x => x.HasInternet, s => s.MapFrom(m => m.HasInternet))
            .ReverseMap();
    }
}


