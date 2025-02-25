using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;

namespace CodeGeneration.Services.Generators
{
    public class ServiceClassGenerator : SingleClassGenerator<Behaviour>, IServiceClassGenerator
    {
        public ServiceClassGenerator(INamingProvider namingProvider, ICodingUnitContextProvider<Behaviour> codingUnitContextProvider) : base(namingProvider, codingUnitContextProvider)
        {
        }
    }
}
