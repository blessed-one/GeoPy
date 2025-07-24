using System.Reflection;
using Application.DTOs;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models;
using Infrastructure.Attributes;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace Infrastructure.Services;

public class ExcelService : IExcelService
{
    private readonly IMapper _mapper;
    
    private readonly IWellRepository _wellRepository;
    private readonly IFieldRepository _fieldRepository;
    
    private readonly ILogger<ExcelService> _logger;

    public ExcelService(
        IMapper mapper,
        IWellRepository wellRepository,
        IFieldRepository fieldRepository,
        ILogger<ExcelService> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _wellRepository = wellRepository;
        _fieldRepository = fieldRepository;

        ExcelPackage.License.SetNonCommercialPersonal("My PC");
    }

    public async Task<ExcelImportResult> ImportFromExcelAsync(Stream stream)
    {
        try
        {
            stream.Position = 0;
            var importData = new ExcelImportData
            {
                Fields = ImportFromExcel<FieldExcelRecord>(stream, "Месторождения"),
                Wells = ImportFromExcel<WellExcelRecord>(stream, "Скважины")
            };

            return await ProcessImportData(importData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при импорте данных из Excel");
            throw;
        }
    }
    
    private List<T> ImportFromExcel<T>(Stream stream, string sheetName) where T : new()
    {
        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets[sheetName];
        
        if (worksheet == null)
        {
            throw new InvalidOperationException($"Рабочий лист {sheetName} не найден");
        }
        
        if (worksheet.Dimension.Rows < 2) 
            return [];
        
        var columnMappings = GetExcelColumnMappings<T>(worksheet);
        return ReadDataRows<T>(worksheet, columnMappings);
    }

    private Dictionary<int, PropertyInfo> GetExcelColumnMappings<T>(ExcelWorksheet worksheet)
    {
        var properties = typeof(T).GetProperties();
        var mappings = new Dictionary<int, PropertyInfo>();
        
        for (var column = 1; column <= worksheet.Dimension.Columns; column++)
        {
            var header = worksheet.Cells[1, column].Value?.ToString()?.Trim();
            
            if (string.IsNullOrEmpty(header)) 
                continue;

            var property = properties.FirstOrDefault(p =>
                p.GetCustomAttribute<ExcelColumnAttribute>()?.Name == header);
            
            if (property != null)
                mappings[column] = property;
        }
        
        return mappings;
    }
    private List<T> ReadDataRows<T>(ExcelWorksheet worksheet, Dictionary<int, PropertyInfo> columnMappings) where T : new()
    {
        var result = new List<T>();
        
        for (var row = 2; row <= worksheet.Dimension.Rows; row++)
        {
            var item = new T();
            var hasData = false;

            foreach (var (column, property) in columnMappings)
            {
                var value = worksheet.Cells[row, column].Value;
                if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
                    continue;

                try
                {
                    SetPropertyValue(item, property, value);
                    hasData = true;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Не удалось преобразовать значение {Value} для свойства {Property}", 
                        value, property.Name);
                }
            }
            
            if (hasData)
                result.Add(item);
        }
        
        return result;
    }

    private void SetPropertyValue(object obj, PropertyInfo property, object value)
    {
        var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        var convertedValue = Convert.ChangeType(value, targetType);
        property.SetValue(obj, convertedValue);
    }
    
    private async Task<ExcelImportResult> ProcessImportData(ExcelImportData importData)
    {
        var result = new ExcelImportResult();
    
        var existingFields = (await _fieldRepository.GetAllAsync())
            .ToDictionary(f => f.FieldName);
    
        var fieldNameToIdMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        
        foreach (var fieldRecord in importData.Fields)
        {
            if (existingFields.TryGetValue(fieldRecord.Name, out var existingField))
            {
                _mapper.Map(fieldRecord, existingField);
                var hasUpdated = await _fieldRepository.UpdateAsync(existingField);
                
                if (hasUpdated)
                    result.FieldsUpdated++;
                else
                    result.FieldsSkipped++;
            }
            else
            {
                var newField = _mapper.Map<Field>(fieldRecord);
                await _fieldRepository.AddAsync(newField);
                result.FieldsAdded++;
            }
            
            fieldNameToIdMap[fieldRecord.Name] = fieldRecord.FieldId;
        }

        var existingWells = (await _wellRepository.GetAllAsync())
            .ToDictionary(w => w.WellId);
    
        foreach (var wellRecord in importData.Wells)
        {
            if (!fieldNameToIdMap.TryGetValue(wellRecord.FieldName, out var fieldId))
            {
                _logger.LogWarning($"Месторождение с именем '{wellRecord.FieldName}' не найдено для скважины {wellRecord.WellId}");
                result.WellsSkipped++;
                continue;
            }
            
            if (existingWells.TryGetValue(wellRecord.WellId, out var existingWell))
            {
                _mapper.Map(wellRecord, existingWell);
                existingWell.FieldId = fieldId;
                
                var hasUpdated = await _wellRepository.UpdateAsync(existingWell);
                
                if (hasUpdated)
                    result.WellsUpdated++;
                else
                    result.WellsSkipped++;
            }
            else
            {
                var wellDto = _mapper.Map<Well>(wellRecord);
                wellDto.FieldId = fieldId; 

                await _wellRepository.AddAsync(wellDto);
                result.WellsAdded++;
            }
        }

        return result;
    }

    public async Task<byte[]> ExportToExcelAsync()
    {
        var wells = await _wellRepository.GetAllAsync();
        var fields = await _fieldRepository.GetAllAsync();

        var fieldNames = fields.ToDictionary(f => f.FieldId, f => f.FieldName);
        
        var wellRecords = wells.Select(w => 
        {
            var record = _mapper.Map<WellExcelRecord>(w);
            record.FieldName = fieldNames.GetValueOrDefault(w.FieldId, "Неизвестно");
            return record;
        });
        
        var fieldRecords = fields.Select(f => _mapper.Map<FieldExcelRecord>(f));

        using var package = new ExcelPackage();

        AddSheetFromData(package, "Скважины", wellRecords);
        AddSheetFromData(package, "Месторождения", fieldRecords);

        return await package.GetAsByteArrayAsync();
    }

    private static void AddSheetFromData<T>(ExcelPackage package, string sheetName, IEnumerable<T> data)
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
                var cell = worksheet.Cells[row, col + 1];

                if (value is DateTime dt)
                {
                    cell.Value = dt;
                    cell.Style.Numberformat.Format = "dd.MM.yyyy";
                }
                else
                {
                    cell.Value = value;
                }
            }
            row++;
        }
        
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
    }
    
    private class ExcelImportData
    {
        public List<FieldExcelRecord> Fields { get; init; } = [];
        public List<WellExcelRecord> Wells { get; init; } = [];
    }
}