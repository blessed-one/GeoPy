namespace Domain.Models;

public class Field
{
    public int FieldId { get; set; }
    public required string FieldName { get; init; }
    public int FieldCode { get; init; }
    public required string AreaName { get; init; }
    
    public List<Well>? Wells { get; init; }
}