using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class SingleInterfaceGenerator : CodeGenerator<Behaviour>
    {
        protected SingleInterfaceGenerator(INamingProvider namingProvider, ICodingUnitContextProvider<Behaviour> codingUnitContextProvider) : base(namingProvider, codingUnitContextProvider)
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
            return NamingProvider.GetName(CodingUnit);
        }
        protected abstract TypeSyntax GetBaseInterface();
    }
}
