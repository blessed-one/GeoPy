using Application.DTOs;
using AutoMapper;
using Domain.Models;

namespace Application;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<Well, WellDto>()
            .ForMember(dest => dest.FieldName, opt => 
                opt.MapFrom(src => src.Field != null ? src.Field.FieldName : null));

        CreateMap<WellDto, Well>();
        
        CreateMap<WellExcelImportRecord, Well>()
            .ForMember(dest => dest.MeasurementDate, 
                opt => opt.MapFrom(src => DateOnly.FromDateTime(src.MeasurementDate)));

        CreateMap<FieldExcelImportRecord, Field>()
            .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.FieldCode, opt =>
            {
                int code;
                opt.MapFrom(src => int.TryParse(src.Code, out code) ? code : 0);
            })
            .ForMember(dest => dest.AreaName, opt => opt.MapFrom(src => src.AreaName ?? string.Empty))
            .ForMember(dest => dest.Wells, opt => opt.Ignore());
    }
}
