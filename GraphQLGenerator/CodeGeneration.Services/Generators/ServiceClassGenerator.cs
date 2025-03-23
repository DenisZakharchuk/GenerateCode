using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;

namespace CodeGeneration.Services.Generators
{
    public class ServiceClassGenerator : SingleClassGenerator<Behaviour>, IServiceClassGenerator
    {
        public ServiceClassGenerator(
            IDeclarationProvider namingProvider,
            ICodingUnitContextProvider<Behaviour> codingUnitContextProvider,
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
