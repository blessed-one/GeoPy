namespace Application.DTOs;

public class WellDto
{
    public int WellId { get; set; }
    public required string WellNumber { get; init; }
    
    public int Debit { get; init; }
    public int Pressure { get; init; }
    public DateOnly MeasurementDate { get; init; } 
    
    public int FieldId { get; init; }
    public string? FieldName { get; init; }
}