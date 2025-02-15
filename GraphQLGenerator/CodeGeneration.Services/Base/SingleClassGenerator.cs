using CodeGeneration.Models.CodingUnits.Providers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class SingleClassGenerator : CodeGenerator
    {
        protected SingleClassGenerator(ICodingUnitInfoProvider classInfoProvider) : base(classInfoProvider)
        {
        }
        protected override IEnumerable<MemberDeclarationSyntax> PrimaryMemberDeclarations()
        {
            var classDeclarationSyntax = SyntaxFactory.ClassDeclaration(GetClassName())
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            if (CodingUnitInfoProvider.HasBase)
            {
                classDeclarationSyntax = classDeclarationSyntax.AddBaseListTypes(
                    GetBaseListTypes());
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

        protected abstract BaseTypeSyntax[] GetBaseListTypes();

        protected virtual string GetClassName()
        {
            return CodingUnitInfoProvider.Name;
        }
    }
}
