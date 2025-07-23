using API.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/wells")]
public class WellsFileController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IExcelService _excelService;
    private readonly ILogger<WellsFileController> _logger;
    
    public WellsFileController(
        IMapper mapper, 
        IExcelService excelService,
        ILogger<WellsFileController> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _excelService = excelService;
    }
    
    [HttpPost("import")]
    public async Task<ActionResult<ImportFileResponse>> ImportFromExcel(IFormFile? file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не был загружен");
            
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            
            var result = await _excelService.ImportFromExcelAsync(stream);
            var response = _mapper.Map<ImportFileResponse>(result);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при импорте скважин из Excel");
            return StatusCode(500, "Internal server error");
        }
    }
    

    [HttpGet("export")]
    public async Task<IActionResult> ExportToExcel()
    {
        try
        {
            var fileContents = await _excelService.ExportToExcelAsync();
            
            return File(fileContents, 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"wells_export_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при экспорте скважин в Excel");
            return StatusCode(500, "Internal server error");
        }
    }
}