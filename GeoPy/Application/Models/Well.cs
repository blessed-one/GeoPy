namespace Core.Models;

public class Well
{
    public int WellId { get; set; }
    public required string WellNumber { get; set; }
    
    public int Debit { get; set; }
    public int Pressure { get; set; }
    public DateOnly MeasurementDate { get; set; } 
    
    public int FieldId { get; set; }
    public Field? Field { get; set; }
}