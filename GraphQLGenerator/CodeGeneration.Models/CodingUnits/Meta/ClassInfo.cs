namespace CodeGeneration.Models.CodingUnits.Meta
{
    public class ClassInfo
    {
        public PropertyInfo[]? Properties { get; set; }
        public required string Name { get; set; }
        public string? Namespace { get; set; }
    }
}
