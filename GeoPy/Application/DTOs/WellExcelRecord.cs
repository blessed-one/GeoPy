using Application.Attributes;

namespace Application.DTOs;

public class WellExcelRecord
{
    [ExcelColumn("идентификатор скважины")]
    public int WellId { get; set; }
    
    [ExcelColumn("номер скважины")]
    public string WellNumber { get; set; }
    
    [ExcelColumn("идентификатор месторождения")]
    public int FieldId { get; set; }
    
    [ExcelColumn("наименование месторождения")]
    public string FieldName { get; set; }
    
    [ExcelColumn("дебит")]
    public int Debit { get; set; }
    
    [ExcelColumn("давление")]
    public int Pressure { get; set; }
    
    [ExcelColumn("дата замера")]
    public DateTime MeasurementDate { get; set; }
}