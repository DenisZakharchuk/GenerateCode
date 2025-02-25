using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    public class DataModelInfoProvider : ModelInfoProvider, IDataModelInfoProvider
    {
        public DataModelInfoProvider(Model codingUnit) : base(codingUnit)
        {
        }
    }
}
