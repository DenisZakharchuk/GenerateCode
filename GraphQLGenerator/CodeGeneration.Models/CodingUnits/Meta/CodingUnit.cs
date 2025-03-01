
using CodeGeneration.Models.CodingUnits.Meta.Members;

namespace CodeGeneration.Models.CodingUnits.Meta
{
    public class Behaviour : Class
    {
    }


    public class Model : Class
    {
    }

    public class Class : CodingUnit
    {
        public PropertyInfo[]? Properties { get; set; }
        public Model? BaseModel { get; set; }
        public IEnumerable<MethodInfo>? Methods { get; set; }
    }

    public class CodingUnit
    {
        public required string Name { get; set; }
        public string? Namespace { get; set; }
    }
}
