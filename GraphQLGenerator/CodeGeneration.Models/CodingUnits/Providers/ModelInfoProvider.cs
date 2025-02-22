using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Providers.Naming;

namespace CodeGeneration.Models.CodingUnits.Providers
{

    public class ModelInfoProvider : CodingUnitInfoProvider<Model>, IModelInfoProvider
    {
        public ModelInfoProvider(Model codingUnit, INamingProvider namingProvider) : base(codingUnit, namingProvider)
        {
        }
    }
}
