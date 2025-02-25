using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class SingleClassGenerator : CodeGenerator<Class>
    {
        protected SingleClassGenerator(
            INamingProvider namingProvider,
            ICodingUnitContextProvider<Class> codingUnitContextProvider) : base(namingProvider, codingUnitContextProvider)
        {
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
