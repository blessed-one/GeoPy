using Application.DTOs;
using AutoMapper;
using Domain.Models;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Well, WellDto>()
            .ForMember(dest => dest.FieldName, opt => 
                opt.MapFrom(src => src.Field != null ? src.Field.FieldName : null));
        
        CreateMap<WellDto, Well>()
            .ForMember(dest => dest.Field, opt => opt.Ignore());
        
    }
}
