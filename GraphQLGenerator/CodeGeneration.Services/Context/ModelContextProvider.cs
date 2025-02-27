using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Services.Context
{
    public class BehaviourContextProvider : CodingUnitContextProvider<Behaviour>, IBehaviourContextProvider
    {
        public override IEnumerable<string> RequiredNamespaces
        {
            get
            {
                if (CodingUnit.Methods != null)
                {
                    foreach (var method in CodingUnit.Methods)
                    {
                        if (method.ReturnType != null && !string.IsNullOrEmpty(method.ReturnType.Namespace))
                        {
                            yield return method.ReturnType.Namespace;
                        }
                    }
                }
            }
        }

        public override bool HasBase => CodingUnit.BaseModel != null;
    }

    public interface IBehaviourContextProvider : ICodingUnitContextProvider<Behaviour>
    {
    }

    public class ModelContextProvider : CodingUnitContextProvider<Model>, IModelContextProvider
    {
        public ModelContextProvider()
        {
        }

        public override IEnumerable<string> RequiredNamespaces
        {
            get
            {
                if (CodingUnit.Properties != null)
                {
                    foreach (var prop in CodingUnit.Properties)
                    {
                        if (!prop.IsPrimitive && prop.PropertyType != null && !string.IsNullOrEmpty(prop.PropertyType.Namespace))
                        {
                            yield return prop.PropertyType.Namespace;
                        }
                    }
                }
            }
        }

        public override bool HasBase => CodingUnit.BaseModel != null;
    }
}
