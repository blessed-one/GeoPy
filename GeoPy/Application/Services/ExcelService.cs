using System.Reflection;
using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace Application.Services;

public class ExcelService : IExcelService
{
    private readonly IMapper _mapper;
    
    private readonly IFieldRepository _fieldRepository;
    private readonly IWellRepository _wellRepository;
    
    private readonly ILogger<ExcelService> _logger;

    public ExcelService(
        IMapper mapper,
        IFieldRepository fieldRepository,
        IWellRepository wellRepository,
        ILogger<ExcelService> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _fieldRepository = fieldRepository;
        _wellRepository = wellRepository;

        ExcelPackage.License.SetNonCommercialPersonal("My PC");
    }

    public async Task<ImportResult> ImportFromExcelAsync(Stream stream)
    {
        stream.Position = 0;

        var fields = await ImportFromExcelAsync<FieldExcelImportRecord>(
            stream, "Месторождения");
            
        stream.Position = 0;
        
        var wells = await ImportFromExcelAsync<WellExcelImportRecord>(
            stream, "Скважины");
            
        var result = await ProcessImportData(fields, wells);
        
        return result;
    }
    
    private async Task<IEnumerable<T>> ImportFromExcelAsync<T>(Stream stream, string sheetName) where T : new()
    {
        try
        {
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[sheetName];
            
            if (worksheet == null)
            {
                throw new InvalidOperationException($"Рабочий лист {sheetName} не найден");
            }
            
            var rowCount = worksheet.Dimension.Rows;
            var colCount = worksheet.Dimension.Columns;
            
            if (rowCount < 2) 
                return [];
            
            var properties = typeof(T).GetProperties();
            var result = new List<T>();
            
            for (var row = 2; row <= rowCount; row++)
            {
                var item = new T();
                var hasData = false;
                
                for (var col = 1; col <= colCount; col++)
                {
                    var header = worksheet.Cells[1, col].Value?.ToString()?.Trim();
                    var value = worksheet.Cells[row, col].Value;
                    
                    if (string.IsNullOrEmpty(header)) 
                        continue;
                    
                    var property = properties.FirstOrDefault(p => 
                        p.GetCustomAttribute<ExcelColumnAttribute>()?.Name == header);

                    if (property == null || value == null) 
                        continue;
                    
                    try
                    {
                        var convertedValue = Convert.ChangeType(value, property.PropertyType);
                        property.SetValue(item, convertedValue);
                        hasData = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error converting value {Value} for property {Property}", value, property.Name);
                    }
                }
                
                if (hasData)
                {
                    result.Add(item);
                }
            }
            
            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при импорте данных из Excel: {sheetName}", sheetName);
            throw;
        }
    }

    private async Task<ImportResult> ProcessImportData(
        IEnumerable<FieldExcelImportRecord> fields,
        IEnumerable<WellExcelImportRecord> wells)
    {
        var result = new ImportResult();
        
        var fieldDict = new Dictionary<string, Field>();

        foreach (var field in fields)
        {
            var existingField = await _fieldRepository.GetByIdAsync(field.FieldId);
            if (existingField == null)
            {
                var newField = _mapper.Map<Field>(field);
                await _fieldRepository.AddAsync(newField);
                fieldDict[field.Name] = newField;
                result.FieldsAdded++;
            }
            else
            {
                fieldDict[field.Name] = existingField;
            }
        }

        foreach (var well in wells)
        {
            if (!fieldDict.TryGetValue(well.FieldName, out var field))
            {
                result.WellsSkipped++;
                continue;
            }

            var existingWell = await _wellRepository.GetByIdAsync(well.WellId);
            if (existingWell == null)
            {
                var newWell = _mapper.Map<Well>(well);
                newWell.FieldId = field.FieldId;
                await _wellRepository.AddAsync(newWell);
                result.WellsAdded++;
            }
            else
            {
                _mapper.Map(well, existingWell);
                existingWell.FieldId = field.FieldId;
                await _wellRepository.UpdateAsync(existingWell);
                result.WellsUpdated++;
            }
        }

        return result;
    }

    public async Task<byte[]> ExportToExcelAsync()
    {
        var wells = await _wellRepository.GetAllAsync();
        var fields = await _fieldRepository.GetAllAsync();

        var wellRecords = wells.Select(w => _mapper.Map<WellExcelImportRecord>(w));
        var fieldRecords = fields.Select(f => _mapper.Map<FieldExcelImportRecord>(f));

        using var package = new ExcelPackage();

        AddSheetFromData(package, "Скважины", wellRecords);
        AddSheetFromData(package, "Месторождения", fieldRecords);

        return await package.GetAsByteArrayAsync();
    }

    private static ExcelWorksheet AddSheetFromData<T>(ExcelPackage package, string sheetName, IEnumerable<T> data)
    {
        var worksheet = package.Workbook.Worksheets.Add(sheetName);

        var properties = typeof(T).GetProperties()
            .Where(p => p.IsDefined(typeof(ExcelColumnAttribute), false))
            .ToArray();

        for (var i = 0; i < properties.Length; i++)
        {
            var header = properties[i].GetCustomAttribute<ExcelColumnAttribute>()?.Name ?? properties[i].Name;
            worksheet.Cells[1, i + 1].Value = header;
        }

        var row = 2;
        foreach (var item in data)
        {
            for (var col = 0; col < properties.Length; col++)
            {
                var value = properties[col].GetValue(item);
                worksheet.Cells[row, col + 1].Value = value;
            }
            row++;
        }

        return worksheet;
    }
}

