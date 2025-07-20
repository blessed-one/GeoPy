namespace API;

using API.DTOs;
using Application.DTOs;
using AutoMapper;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<CreateWellRequest, WellDto>();
        CreateMap<UpdateWellRequest, WellDto>();
        
        CreateMap<WellDto, CreateWellResponse>();
        CreateMap<ImportResult, ImportFileResponse>();
    }
}
