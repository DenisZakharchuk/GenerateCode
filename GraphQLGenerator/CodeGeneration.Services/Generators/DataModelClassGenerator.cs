using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;

namespace CodeGeneration.Services.Generators
{
    public class DataModelClassGenerator : SingleClassGenerator, IDataModelClassGenerator
    {
        public DataModelClassGenerator(INamingProvider namingProvider, ICodingUnitContextProvider<Class> codingUnitContextProvider) : base(namingProvider, codingUnitContextProvider)
        {
        }

        public override void Init(Class codingUnit)
        {
        }
    }
}
