using System;
using PropertyListingAPI.DTOs;
using AutoMapper;
using PropertyListingAPI.Models;

namespace PropertyListingAPI.Mappings;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PropertyCreateDto, Property>();
        CreateMap<Property, PropertyReadDto>()
            .ForMember(dest => dest.AgentName, opt => opt.MapFrom(src => src.Agent.FullName));

        CreateMap<ViewingRequest, ViewingRequestReadDto>()
        .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(src => src.Property.Title))
        .ForMember(dest => dest.TenantName, opt => opt.MapFrom(src => src.Tenant.FullName));

    }
}

