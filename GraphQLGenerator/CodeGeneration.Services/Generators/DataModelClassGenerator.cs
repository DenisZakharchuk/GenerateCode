using CodeGeneration.Models.CodingUnits.Providers;
using CodeGeneration.Services.Base;

namespace CodeGeneration.Services.Generators
{
    public class DataModelClassGenerator : SingleClassGenerator, IDataModelClassGenerator
    {
        public DataModelClassGenerator(IDataModelInfoProvider classInfoProvider, IDataModelInfoProvider baseModelInfoProvider) : base(classInfoProvider, baseModelInfoProvider)
        {
        }
    }
}
