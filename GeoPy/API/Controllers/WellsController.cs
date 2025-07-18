using API.DTOs;
using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/wells")]
public class WellsController : ControllerBase
{
    private readonly IWellService _wellService;
    private readonly IMapper _mapper;
    private readonly ILogger<WellsController> _logger;
    
    public WellsController(
        IMapper mapper, 
        IWellService wellService,
        ILogger<WellsController> logger) 
    {
        _mapper = mapper;
        _logger = logger;
        _wellService = wellService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WellDto>>> GetAll()
    {
        try
        {
            var wells = await _wellService.GetAllWellsAsync();
            var result = _mapper.Map<List<WellDto>>(wells);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при получении всех скважин");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WellDto>> GetById(int id)
    {
        try
        {
            var well = await _wellService.GetWellAsync(id);

            if (well == null)
                return NotFound();

            var result = _mapper.Map<WellDto>(well);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при получении скважины по ID : {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<CreateWellResponse>> Create(CreateWellRequest request)
    {
        try
        {
            var well = _mapper.Map<WellDto>(request);
            var createdWell = await _wellService.CreateWellAsync(well);
            var result = _mapper.Map<CreateWellResponse>(createdWell);
            
            return CreatedAtAction(nameof(GetById), new { id = createdWell.WellId }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при создании скважины");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateWellRequest request)
    {
        try
        {
            var wellDto = _mapper.Map<WellDto>(request);
            wellDto.WellId = id;
            
            await _wellService.UpdateWellAsync(wellDto);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при обновлении скважины: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _wellService.DeleteWellAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при удалении скважины: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}