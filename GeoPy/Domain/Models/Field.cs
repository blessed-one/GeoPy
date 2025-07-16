namespace Domain.Models;

public class Field
{
    public int FieldId { get; set; }
    public required string FieldName { get; set; }
    public int FieldCode { get; set; }
    public required string AreaName { get; set; }
    
    public List<Well>? Wells { get; set; }
}