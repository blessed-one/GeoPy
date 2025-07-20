using System.Reflection;
using Application.Attributes;
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
    
    private readonly IWellService _wellService;
    private readonly IFieldRepository _fieldRepository;
    
    private readonly ILogger<ExcelService> _logger;

    public ExcelService(
        IMapper mapper,
        IWellService wellService,
        IFieldRepository fieldRepository,
        ILogger<ExcelService> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _wellService = wellService;
        _fieldRepository = fieldRepository;

        ExcelPackage.License.SetNonCommercialPersonal("My PC");
    }

    public async Task<ImportResult> ImportFromExcelAsync(Stream stream)
    {
        stream.Position = 0;

        var fields = ImportFromExcel<FieldExcelRecord>(
            stream, "Месторождения");
            
        stream.Position = 0;
        
        var wells = ImportFromExcel<WellExcelRecord>(
            stream, "Скважины");
            
        var result = await ProcessImportData(fields, wells);
        
        return result;
    }
    
    private List<T> ImportFromExcel<T>(Stream stream, string sheetName) where T : new()
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
            
            var headerMap = new Dictionary<int, PropertyInfo>();

            for (var column = 1; column <= colCount; column++)
            {
                var header = worksheet.Cells[1, column].Value?.ToString()?.Trim();

                if (string.IsNullOrEmpty(header))
                    continue;

                var property = properties.FirstOrDefault(p =>
                    p.GetCustomAttribute<ExcelColumnAttribute>()?.Name == header);

                if (property != null)
                    headerMap[column] = property;
            }

            for (var row = 2; row <= rowCount; row++)
            {
                var item = new T();
                var hasData = false;

                foreach (var (column, property) in headerMap)
                {
                    var value = worksheet.Cells[row, column].Value;
                    
                    if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
                        continue;

                    try
                    {
                        var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        var convertedValue = Convert.ChangeType(value, targetType);
                        
                        property.SetValue(item, convertedValue);
                        
                        hasData = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Не удалось преобразовать значение {Value} для свойства {Property}", value, property.Name);
                    }
                }
                
                if (hasData)
                    result.Add(item);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка при импорте данных из Excel с листа {sheetName}", sheetName);
            throw;
        }
    }

    private async Task<ImportResult> ProcessImportData(
        IEnumerable<FieldExcelRecord> fields,
        IEnumerable<WellExcelRecord> wells)
    {
        var result = new ImportResult();
        
        var existingFieldIds = (await _fieldRepository.GetAllAsync())
            .ToDictionary(f => f.FieldId, f => f);
        
        foreach (var fieldRecord in fields)
        {
            if (existingFieldIds.TryGetValue(fieldRecord.FieldId, out var existingField))
            {
                _mapper.Map(fieldRecord, existingField);
                await _fieldRepository.UpdateAsync(existingField);
               
                result.FieldsUpdated++;
            }
            else
            {
                var newField = _mapper.Map<Field>(fieldRecord);
                await _fieldRepository.AddAsync(newField);
                
                existingFieldIds[newField.FieldId] = newField;
                
                result.FieldsAdded++;
            }
        }

        var existingWellIds = (await _wellService.GetAllWellsAsync())
            .ToDictionary(w => w.WellId, w => w);

        foreach (var wellRecord in wells)
        {
            if (existingWellIds.TryGetValue(wellRecord.WellId, out var existingWellDto))
            {
                _mapper.Map(wellRecord, existingWellDto);
                await _wellService.UpdateWellAsync(existingWellDto);
                result.WellsUpdated++;
            }
            else
            {
                var wellDto = _mapper.Map<WellDto>(wellRecord);
                await _wellService.CreateWellAsync(wellDto);
                result.WellsAdded++;
            }
        }

        return result;
    }
    
    public async Task<byte[]> ExportToExcelAsync()
    {
        var wells = await _wellService.GetAllWellsAsync();
        var fields = await _fieldRepository.GetAllAsync();

        var wellRecords = wells.Select(w => _mapper.Map<WellExcelRecord>(w));
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
}

