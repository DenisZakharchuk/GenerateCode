using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers
{

    public class ModelInfoProvider : CodingUnitInfoProvider<Model>, IModelInfoProvider
    {
        public ModelInfoProvider(Model codingUnit) : base(codingUnit)
        {
        }
    }
}
