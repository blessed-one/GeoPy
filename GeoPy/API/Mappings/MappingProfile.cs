using API.DTOs;
using API.DTOs.Requests;
using API.DTOs.Responses;
using Application.DTOs;
using AutoMapper;

namespace API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateWellRequest, WellDto>();
        CreateMap<UpdateWellRequest, WellDto>();
        
        CreateMap<WellDto, CreateWellResponse>();
        CreateMap<ExcelImportResult, ImportFileResponse>();
        
        
    }
}
