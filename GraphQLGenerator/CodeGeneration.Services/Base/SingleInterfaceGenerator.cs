using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class SingleInterfaceGenerator : CodeGenerator<Behaviour>
    {
        protected SingleInterfaceGenerator(IDeclarationProvider namingProvider, ICodingUnitContextProvider<Behaviour> codingUnitContextProvider) : base(namingProvider, codingUnitContextProvider)
        {
        }

        protected override IEnumerable<MemberDeclarationSyntax> PrimaryMemberDeclarations()
        {
            var interfaceDeclarationSyntax = SyntaxFactory.InterfaceDeclaration(DeclarationProvider.GetName())
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            if (DeclarationProvider.HasBase)
            {
                interfaceDeclarationSyntax = interfaceDeclarationSyntax.AddBaseListTypes(
                    SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(DeclarationProvider.GetBaseName())));
            }
                            
            yield return interfaceDeclarationSyntax;
        }
    }
}
