namespace Application.DTOs;

public class ExcelImportResult
{
    public int FieldsAdded { get; set; }
    public int FieldsUpdated { get; set; }
    public int FieldsSkipped { get; set; }
    public int WellsAdded { get; set; }
    public int WellsUpdated { get; set; }
    public int WellsSkipped { get; set; }
}