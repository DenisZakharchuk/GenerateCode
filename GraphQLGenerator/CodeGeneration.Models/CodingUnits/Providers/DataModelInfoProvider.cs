using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    public class DataModelInfoProvider : ClassInfoProvider, IDataModelInfoProvider
    {
        public DataModelInfoProvider(CodingUnit info) : base(info)
        {
        }
    }
}
