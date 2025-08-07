using System;
using System.Linq;
using PropertyListingAPI.DTOs;
using AutoMapper;
using PropertyListingAPI.Models;

namespace PropertyListingAPI.Mappings;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Property mappings
        CreateMap<PropertyCreateDto, Property>()
            .ForMember(dest => dest.Images, opt => opt.Ignore()); // Handle images separately in controller
        
        CreateMap<Property, PropertyReadDto>()
            .ForMember(dest => dest.AgentName, opt => opt.MapFrom(src => src.Agent.FullName))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.OrderBy(i => i.DisplayOrder)))
            .ForMember(dest => dest.PrimaryImageUrl, opt => opt.MapFrom(src => 
                src.Images.FirstOrDefault(i => i.IsPrimary) != null 
                    ? src.Images.FirstOrDefault(i => i.IsPrimary).ImageUrl 
                    : src.Images.OrderBy(i => i.DisplayOrder).FirstOrDefault() != null
                        ? src.Images.OrderBy(i => i.DisplayOrder).FirstOrDefault().ImageUrl
                        : string.Empty));

        // PropertyImage mappings
        CreateMap<PropertyImage, PropertyImageDto>();

        // User mappings
        CreateMap<User, UserReadDto>();
        
        // ViewingRequest mappings
        CreateMap<ViewingRequest, ViewingRequestReadDto>()
            .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(src => src.Property.Title))
            .ForMember(dest => dest.TenantName, opt => opt.MapFrom(src => src.Tenant.FullName));
    }
}

