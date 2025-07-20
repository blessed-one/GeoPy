namespace Application.DTOs;

public class FieldExcelImportRecord
{
    [ExcelColumn("идентификатор месторождения")]
    public int FieldId { get; set; }
    
    [ExcelColumn("наименование месторождения")]
    public string Name { get; set; }
    
    [ExcelColumn("код месторождения")]
    public int Code { get; set; }
    
    [ExcelColumn("наименование площади")]
    public string? AreaName { get; set; }
}