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

        CreateMap<Well, WellExcelRecord>()  
            .ForMember(dest => dest.MeasurementDate, opt =>
                opt.MapFrom(src => src.MeasurementDate.ToDateTime(TimeOnly.MinValue)))
            .ForMember(dest => dest.FieldName, opt =>
                opt.MapFrom(src => src.Field != null ? src.Field.FieldName : null));

        CreateMap<Field, FieldExcelRecord>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FieldName))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.FieldCode));
        
        CreateMap<WellDto, Well>();
        
        CreateMap<WellDto, WellExcelRecord>()
            .ForMember(dest => dest.MeasurementDate, opt => opt.MapFrom(src => src.MeasurementDate.ToDateTime(TimeOnly.MinValue)));

        CreateMap<WellExcelRecord, Well>()
            .ForMember(dest => dest.MeasurementDate, 
                opt => opt.MapFrom(src => DateOnly.FromDateTime(src.MeasurementDate)));

        CreateMap<WellExcelRecord, WellDto>()
            .ForMember(dest => dest.MeasurementDate, 
                opt => opt.MapFrom(src => DateOnly.FromDateTime(src.MeasurementDate)));
        
        CreateMap<FieldExcelRecord, Field>()
            .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.FieldCode, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Wells, opt => opt.Ignore());
    }
}
