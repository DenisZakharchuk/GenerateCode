using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;

namespace CodeGeneration.Services.Base
{
    public class ModelGenerator : SingleClassGenerator<Model>, IModelGenerator
    {
        public ModelGenerator(INamingProvider namingProvider, IModelContextProvider modelContextProvider) : base(namingProvider, modelContextProvider)
        {
        }
    }
}
