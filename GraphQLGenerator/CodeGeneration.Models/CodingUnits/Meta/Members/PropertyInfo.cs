namespace CodeGeneration.Models.CodingUnits.Meta.Members
{
    public class MethodInfo : BaseMember
    {

    }
    public class PropertyInfo : BaseMember
    {
        public bool IsNullable { get; set; }
        public bool IsCollection { get; set; }
        public List<string>? GenericArguments { get; set; }
        public bool IsPrimitive { get; set; }
        public List<PropertyInfo>? Includes { get; set; }
    }
}
