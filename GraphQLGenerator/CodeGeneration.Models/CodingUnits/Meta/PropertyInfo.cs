namespace CodeGeneration.Models.CodingUnits.Meta
{
    public class PropertyInfo
    {
        public required string Name { get; set; }
        public required string Type { get; set; }
        public bool IsNullable { get; set; }
        public bool IsCollection { get; set; }
        public List<string>? GenericArguments { get; set; }
        public bool IsPrimitive { get; set; }
        public List<PropertyInfo>? Includes { get; set; }
    }
}
