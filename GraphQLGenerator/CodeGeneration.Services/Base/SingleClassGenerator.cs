using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class SingleClassGenerator<TCodingUnit> : CodeGenerator<TCodingUnit>
        where TCodingUnit: Class
    {
        private readonly IMemberGenerator<PropertyInfo> _propertyGenerator;
        private readonly IMemberGenerator<MethodInfo> _methodGenerator;

        protected SingleClassGenerator(
            INamingProvider namingProvider,
            ICodingUnitContextProvider<TCodingUnit> codingUnitContextProvider,
            IMemberGenerator<PropertyInfo> propertyGenerator,
            IMemberGenerator<MethodInfo> methodGenerator) : base(namingProvider, codingUnitContextProvider)
        {
            _propertyGenerator = propertyGenerator;
            _methodGenerator = methodGenerator;
        }

        protected override IEnumerable<MemberDeclarationSyntax> PrimaryMemberDeclarations()
        {
            var classDeclarationSyntax = SyntaxFactory.ClassDeclaration(GetClassName())
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            if (CodingUnitContextProvider.HasBase)
            {
                classDeclarationSyntax = classDeclarationSyntax.AddBaseListTypes(
                    GetBaseTypes().ToArray());
            }
            
            classDeclarationSyntax = classDeclarationSyntax.AddMembers(
                GetMembers().ToArray());

            yield return classDeclarationSyntax;
        }

        protected virtual IEnumerable<MemberDeclarationSyntax> GetMembers()
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

        protected virtual ConstructorDeclarationSyntax GenerateConstructor()
        {
            return SyntaxFactory.ConstructorDeclaration(GetClassName())
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block());
        }

        protected virtual IEnumerable<BaseTypeSyntax> GetBaseTypes()
        {
            Model? baseModel = CodingUnit.BaseModel;
            if (baseModel != null)
            {
                var name = SyntaxFactory.ParseTypeName(NamingProvider.GetName(baseModel));
                yield return SyntaxFactory.SimpleBaseType(name);
            }
        }

        protected virtual string GetClassName()
        {
            return NamingProvider.GetName(CodingUnit);
        }
    }
}
