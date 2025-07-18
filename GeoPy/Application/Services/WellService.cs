using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class WellService : IWellService
{
    private readonly IMapper _mapper;
    private readonly IWellRepository _repository;
    private readonly ILogger<WellService> _logger;

    public WellService(
        IMapper mapper, 
        IWellRepository repository,
        ILogger<WellService> logger)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<WellDto?> GetWellAsync(int id)
    {
        var well = await _repository.GetByIdAsync(id);
        return _mapper.Map<WellDto>(well);
    }

    public async Task<IEnumerable<WellDto>> GetAllWellsAsync()
    {
        var wells = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<WellDto>>(wells);
    }

    public async Task<WellDto> CreateWellAsync(WellDto dto)
    {
        var well = _mapper.Map<Well>(dto);
        await _repository.AddAsync(well);
        return _mapper.Map<WellDto>(well);
    }

    public async Task UpdateWellAsync(WellDto dto)
    {
        var well = _mapper.Map<Well>(dto);
        await _repository.UpdateAsync(well);
    }

    public async Task DeleteWellAsync(int id)
    {
        var well = await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<WellDto>> GetWellsByFieldAsync(int fieldId)
    {
        throw new NotImplementedException();
    }
}