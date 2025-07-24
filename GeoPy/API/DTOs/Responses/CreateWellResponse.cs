namespace API.DTOs.Responses;

public sealed record CreateWellResponse(
    string WellNumber, 
    int Debit, 
    int Pressure, 
    DateOnly MeasurementDate, 
    int FieldId
);