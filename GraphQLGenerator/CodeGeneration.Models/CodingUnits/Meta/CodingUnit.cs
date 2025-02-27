
namespace CodeGeneration.Models.CodingUnits.Meta
{
    public class Behaviour : Class
    {
        public IEnumerable<MethodInfo>? Methods { get; set; }
    }

    public class MethodInfo
    {
        public required string Name { get; set; }
        public CodingUnit? ReturnType { get; set; }
    }

    public class Model : Class
    {
        public PropertyInfo[]? Properties { get; set; }
    }

    public class Class : CodingUnit
    {
        public Model? BaseModel { get; set; }
    }

    public class CodingUnit
    {
        public required string Name { get; set; }
        public string? Namespace { get; set; }
    }
}
