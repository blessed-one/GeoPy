using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public record CreateWellResponse(string WellNumber, int Debit, int Pressure, DateOnly MeasurementDate, int FieldId);