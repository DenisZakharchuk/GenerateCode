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
        public IEnumerable<CodingUnit>? GenericTypeParameters { get; set; }
        public PropertyInfo[]? Properties { get; set; }
        public Model? BaseModel { get; set; }
        public IEnumerable<MethodInfo>? Methods { get; set; }
        public IEnumerable<CodingUnit>? GenericTypeArguments { get; set; }

        public override IEnumerable<string> GetNamespaces()
        {
            foreach(var _namespace in base.GetNamespaces())
            {
                yield return _namespace;
            }

            if(BaseModel != null && !string.IsNullOrEmpty(BaseModel.Namespace))
            {
                yield return BaseModel.Namespace;
            }

            if(GenericTypeArguments != null)
            {
                foreach (var _genericType in GenericTypeArguments)
                {
                    if (!string.IsNullOrEmpty(_genericType.Namespace))
                    {
                        yield return _genericType.Namespace;
                    }
                }
            }
        }
    }

    public class CodingUnit
    {
        public required string Name { get; set; }
        public string? Namespace { get; set; }
        public bool IsCollection { get; set; }
        public bool IsPrimitive { get; set; }

        public virtual IEnumerable<string> GetNamespaces()
        {
            //throw new NotImplementedException();
            if (!string.IsNullOrEmpty(Namespace)) yield return Namespace;
            if (IsCollection) yield return "System.Collections.Generic";
        }
    }
}
