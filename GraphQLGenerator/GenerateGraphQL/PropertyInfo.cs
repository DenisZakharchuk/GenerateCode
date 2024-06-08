using System.Collections.Generic;

namespace GQLG
{
    public class PropertyInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsNullable { get; set; }
        public bool IsCollection { get; set; }
        public List<string> GenericArguments { get; set; }
    }
}