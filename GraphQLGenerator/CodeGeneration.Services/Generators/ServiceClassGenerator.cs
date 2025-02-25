using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;

namespace CodeGeneration.Services.Generators
{
    public class ServiceClassGenerator : SingleClassGenerator, IServiceClassGenerator
    {
        public ServiceClassGenerator(INamingProvider namingProvider, ICodingUnitContextProvider<Class> codingUnitContextProvider) : base(namingProvider, codingUnitContextProvider)
        {
        }
    }
}
