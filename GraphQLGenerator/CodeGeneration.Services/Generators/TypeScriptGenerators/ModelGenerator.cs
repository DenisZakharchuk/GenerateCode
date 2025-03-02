using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;

namespace CodeGeneration.Services.Generators.TypeScriptGenerators
{
    public class ModelGenerator : SingleClassGenerator<Model>, IModelGenerator
    {
        public ModelGenerator(INamingProvider namingProvider, ICodingUnitContextProvider<Model> codingUnitContextProvider, IMemberGenerator<PropertyInfo> propertyGenerator, IMemberGenerator<MethodInfo> methodGenerator) : base(namingProvider, codingUnitContextProvider, propertyGenerator, methodGenerator)
        {
        }
    }
}
