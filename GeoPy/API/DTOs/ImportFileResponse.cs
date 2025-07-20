namespace API.DTOs;

public record ImportFileResponse(int FieldsAdded, int FieldsUpdated, int WellsAdded, int WellsUpdated, int WellsSkipped);