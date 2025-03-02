using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;

namespace CodeGeneration.Services.Generators
{
    public class ModelGenerator : SingleClassGenerator<Model>, IModelGenerator
    {
        public ModelGenerator(
            IDefaultNamingProvider namingProvider,
            IModelContextProvider modelContextProvider,
            IMemberGenerator<PropertyInfo> propertyGenerator,
            IMemberGenerator<MethodInfo> methodGenerator
            ) : base(namingProvider, modelContextProvider, propertyGenerator, methodGenerator)
        {
        }
    }
}
