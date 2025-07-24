namespace API.DTOs.Responses;

public sealed record ImportFileResponse(
    int FieldsAdded, 
    int FieldsUpdated, 
    int FieldsSkipped,
    int WellsAdded, 
    int WellsUpdated, 
    int WellsSkipped
);