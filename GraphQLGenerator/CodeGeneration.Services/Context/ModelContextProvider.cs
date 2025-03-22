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
                        if (method.Type != null && !string.IsNullOrEmpty(method.Type.Namespace))
                        {
                            yield return method.Type.Namespace;
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
                        if (prop.Type != null)
                        {
                            var propertyType = prop.Type;

                            foreach(var namespaces in propertyType.GetNamespaces())
                            {
                                yield return namespaces;
                            }
                        }
                    }
                }
                if (CodingUnit.Methods != null)
                {
                    foreach (var method in CodingUnit.Methods)
                    {
                        if(method.Type != null)
                        {
                            var propertyType = method.Type;

                            foreach (var namespaces in propertyType.GetNamespaces())
                            {
                                yield return namespaces;
                            }
                        }
                    }
                }
            }
        }

        public override bool HasBase => CodingUnit.BaseModel != null;
    }
}
