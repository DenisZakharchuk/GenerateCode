using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using CodeGeneration.Services.Base.Result;

namespace CodeGeneration.Services.Generators
{
    
    public class PropertyGenerator : MemberGenerator<PropertyInfo>, IPropertyGenerator
    {
        private readonly IDeclarationGenerator<PropertyInfo, TypeSyntax> _declarationGenerator;

        public PropertyGenerator(IDeclarationGenerator<PropertyInfo, TypeSyntax> declarationGenerator)
        {
            _declarationGenerator = declarationGenerator;
        }

        public IDeclarationGenerator<PropertyInfo, TypeSyntax> DeclarationGenerator => _declarationGenerator;

        public override GenerationResult<MemberDeclarationSyntax> Generate()
        {
            return new GenerationResult<MemberDeclarationSyntax>(CreateMemberDeclaration(CodingUnit));
        }

        protected override PropertyDeclarationSyntax CreateMemberDeclaration(PropertyInfo memberInfo)
        {
            _declarationGenerator.Init(memberInfo);
            TypeSyntax propertyTypeDeclaration = _declarationGenerator.Generate().Result;// GetDeclaration(memberInfo);

            var property = SyntaxFactory.PropertyDeclaration(
                propertyTypeDeclaration, memberInfo.Name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(
                [
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration, default, default(SyntaxTokenList), SyntaxFactory.Token(SyntaxKind.GetKeyword), default, default, SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration, default, default(SyntaxTokenList), SyntaxFactory.Token(SyntaxKind.SetKeyword), default, default, SyntaxFactory.Token(SyntaxKind.SemicolonToken)).AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)),
                ]))
            );
            return property;
        }
    }
}
