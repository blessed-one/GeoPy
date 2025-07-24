using AutoMapper;
using Domain.Models;
using Infrastructure.Models;

namespace Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Well, WellExcelRecord>()
            .ForMember(dest => dest.MeasurementDate, opt =>
                opt.MapFrom(src => src.MeasurementDate.ToDateTime(TimeOnly.MinValue)));
            
        CreateMap<Field, FieldExcelRecord>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FieldName))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.FieldCode));
        
        CreateMap<WellExcelRecord, Well>()
            .ForMember(dest => dest.MeasurementDate, 
                opt => opt.MapFrom(src => DateOnly.FromDateTime(src.MeasurementDate)));

        CreateMap<FieldExcelRecord, Field>()
            .ForMember(dest => dest.FieldName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.FieldCode, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Wells, opt => opt.Ignore());
    }
}
