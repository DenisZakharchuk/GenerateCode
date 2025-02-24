namespace CodeGeneration.Models.CodingUnits.Meta
{
    public class Behaviour : CodingUnit
    {

    }
    public class Model : CodingUnit
    {
        public PropertyInfo[]? Properties { get; set; }
    }
    public class CodingUnit
    {
        public required string Name { get; set; }
        public string? Namespace { get; set; }
    }
}
