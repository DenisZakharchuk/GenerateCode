using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class GenericClassGenerator<TCodingUnit> : SingleClassGenerator<TCodingUnit>
        where TCodingUnit : Class
    {

        protected GenericClassGenerator(
            IDeclarationProvider namingProvider,
            ICodingUnitContextProvider<TCodingUnit> codingUnitContextProvider,
            IMemberGenerator<PropertyInfo> propertyGenerator,
            IMemberGenerator<MethodInfo> methodGenerator) : base(namingProvider, codingUnitContextProvider, propertyGenerator, methodGenerator)
        {
        }
    }
    public abstract class SingleClassGenerator<TCodingUnit> : CodeGenerator<TCodingUnit>
        where TCodingUnit: Class
    {
        private readonly IMemberGenerator<PropertyInfo> _propertyGenerator;
        private readonly IMemberGenerator<MethodInfo> _methodGenerator;


        protected SingleClassGenerator(
            IDeclarationProvider namingProvider,
            ICodingUnitContextProvider<TCodingUnit> codingUnitContextProvider,
            IMemberGenerator<PropertyInfo> propertyGenerator,
            IMemberGenerator<MethodInfo> methodGenerator) : base(namingProvider, codingUnitContextProvider)
        {
            _propertyGenerator = propertyGenerator;
            _methodGenerator = methodGenerator;
        }

        protected override IEnumerable<MemberDeclarationSyntax> GetMembers()
        {
            yield return GenerateConstructor();
            if (CodingUnit.Properties != null)
            {
                foreach (var propertyInfo in CodingUnit.Properties)
                {
                    _propertyGenerator.Init(propertyInfo);
                    yield return _propertyGenerator.GenerateMember();
                }
            }
            if (CodingUnit.Methods != null)
            {
                foreach (var methodInfo in CodingUnit.Methods)
                {
                    _methodGenerator.Init(methodInfo);
                    yield return _methodGenerator.GenerateMember();
                }
            }
        }
    }
}
