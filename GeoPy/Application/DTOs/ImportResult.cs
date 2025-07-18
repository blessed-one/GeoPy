namespace Application.DTOs;

public class ImportResult
{
    public int FieldsAdded { get; set; }
    public int WellsAdded { get; set; }
    public int WellsUpdated { get; set; }
    public int WellsSkipped { get; set; }
}