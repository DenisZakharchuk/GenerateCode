using CodeGeneration.Models.CodingUnits.Providers;
using CodeGeneration.Services.Base;

namespace CodeGeneration.Services.Generators
{
    public class ServiceClassGenerator : SingleClassGenerator, IServiceClassGenerator
    {
        public ServiceClassGenerator(IModelInfoProvider classInfoProvider, ICodingUnitInfoProvider baseClassInfoProvider) : base(classInfoProvider, baseClassInfoProvider)
        {
        }
    }
}
