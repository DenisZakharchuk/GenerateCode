using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Generators
{
    public class ModelGenerator : SingleClassGenerator<Model>, IModelGenerator
    {
        public static Dictionary<string, SyntaxKind> Mapping = new Dictionary<string, SyntaxKind>() {
            { "Integer", SyntaxKind.IntKeyword },
            { "String", SyntaxKind.StringKeyword }
        };

        public ModelGenerator(IDefaultNamingProvider namingProvider, IModelContextProvider modelContextProvider) : base(namingProvider, modelContextProvider)
        {
        }

        protected override IEnumerable<MemberDeclarationSyntax> PrimaryMemberDeclarations()
        {
            foreach (var memberDeclaration in base.PrimaryMemberDeclarations())
            {
                if (memberDeclaration is ClassDeclarationSyntax classDeclaration)
                {
                    if (CodingUnit.Properties != null)
                    {
                        foreach (var propertyInfo in CodingUnit.Properties)
                        {
                            if (propertyInfo.PropertyType != null)
                            {
                                var PropertyName = propertyInfo.Name;
                                var PropertyTypeSyntaxKind = GetPropertyTypeSyntaxKind(propertyInfo.PropertyType);

                                var property = CreatePropertyDeclaration(PropertyName, PropertyTypeSyntaxKind);

                                classDeclaration = classDeclaration.AddMembers(property);
                            }
                        }
                    }
                    yield return classDeclaration;
                }
                else
                {
                    yield return memberDeclaration;
                }
            }
        }

        private SyntaxKind GetPropertyTypeSyntaxKind(CodingUnit propertyType)
        {
            if (propertyType is null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            return Mapping.ContainsKey(propertyType.Name)
                ? Mapping[propertyType.Name]
                : throw new ApplicationException($"Cannot provide the property type for {propertyType.Name}");
        }

        private PropertyDeclarationSyntax CreatePropertyDeclaration(string PropertyName, SyntaxKind PropertyTypeSyntaxKind)
        {
            var property = SyntaxFactory.PropertyDeclaration(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(PropertyTypeSyntaxKind)), PropertyName)
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
