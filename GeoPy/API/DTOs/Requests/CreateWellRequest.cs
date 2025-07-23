using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Requests;

public sealed record CreateWellRequest
(
    [Required] string WellNumber,
    [Range(0, int.MaxValue)] int Debit,
    [Range(0, int.MaxValue)] int Pressure,
    DateOnly MeasurementDate,
    [Range(1, int.MaxValue)] int FieldId
);