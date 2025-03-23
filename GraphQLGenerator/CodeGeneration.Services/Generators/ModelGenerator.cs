using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;

namespace CodeGeneration.Services.Generators
{
    public class ModelGenerator : SingleClassGenerator<Model>, IModelGenerator
    {
        //public ModelGenerator(
        //    IDefaultDeclarationProvider namingProvider,
        //    IModelContextProvider modelContextProvider,
        //    IMemberGenerator<PropertyInfo> propertyGenerator,
        //    IMemberGenerator<MethodInfo> methodGenerator
        //    ) : base(namingProvider, modelContextProvider, propertyGenerator, methodGenerator)
        //{
        //}
        public ModelGenerator(
            IDeclarationProvider namingProvider,
            ICodingUnitContextProvider<Model> codingUnitContextProvider,
            IMemberGenerator<PropertyInfo> propertyGenerator,
            IMemberGenerator<MethodInfo> methodGenerator,
            ITypeParameterGenerator<CodingUnit> typeParameterGenerator,
            ITypeParameterConstraintGenerator<Class> typeParameterConstraintGenerator) 
            : base(
                  namingProvider,
                  codingUnitContextProvider,
                  propertyGenerator,
                  methodGenerator,
                  typeParameterGenerator,
                  typeParameterConstraintGenerator)
        {
        }
    }
}
