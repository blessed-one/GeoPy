using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateWellRequest
{
    [Required] public required string WellNumber { get; init; }
    
    [Range(0, int.MaxValue)] public int Debit { get; init; }
    [Range(0, int.MaxValue)] public int Pressure { get; init; }
   
    public DateOnly MeasurementDate { get; init; }
    
    [Range(1, int.MaxValue)] public int FieldId { get; init; }
}