using CodeGeneration.Models.CodingUnits.Providers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class SingleInterfaceGenerator : CodeGenerator
    {
        protected SingleInterfaceGenerator(IBehaviourInfoProvider classInfoProvider) : base(classInfoProvider)
        {
        }

        protected override IEnumerable<MemberDeclarationSyntax> PrimaryMemberDeclarations()
        {
            var graphQLTypeName = GetInterfaceName();
            var generatedBaseType = GetBaseInterface();

            yield return SyntaxFactory.InterfaceDeclaration(graphQLTypeName)
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .AddBaseListTypes(
                                SyntaxFactory.SimpleBaseType(generatedBaseType));
        }

        protected virtual string GetInterfaceName()
        {
            return CodingUnitInfoProvider.Name.StartsWith('I')
                ? CodingUnitInfoProvider.Name
                : $"I{CodingUnitInfoProvider.Name}";
        }
        protected abstract TypeSyntax GetBaseInterface();
    }
}
