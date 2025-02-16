using CodeGeneration.Models.CodingUnits.Providers;
using CodeGeneration.Services.Base;

namespace CodeGeneration.Services.Data
{
    public class DataModelClassGenerator : SingleClassGenerator
    {
        public DataModelClassGenerator(IDataModelInfoProvider classInfoProvider) : base(classInfoProvider)
        {
        }
    }
}
