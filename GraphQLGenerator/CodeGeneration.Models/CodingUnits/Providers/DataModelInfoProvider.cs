using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Naming;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    public class DataModelInfoProvider : ModelInfoProvider, IDataModelInfoProvider
    {
        public DataModelInfoProvider(Model codingUnit, INamingProvider namingProvider) : base(codingUnit, namingProvider)
        {
        }
    }
}
