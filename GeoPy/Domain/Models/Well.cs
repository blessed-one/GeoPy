namespace Domain.Models;

public class Well
{
    public int WellId { get; set; }
    public required string WellNumber { get; init; }
    
    public int Debit { get; init; }
    public int Pressure { get; init; }
    public DateOnly MeasurementDate { get; init; } 
    
    public int FieldId { get; set; }
    public Field? Field { get; init; }
}