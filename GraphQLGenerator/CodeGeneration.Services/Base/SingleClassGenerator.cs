using CodeGeneration.Models.CodingUnits.Providers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class SingleClassGenerator : CodeGenerator
    {
        private readonly ICodingUnitInfoProvider _baseClassInfoProvider;

        public ICodingUnitInfoProvider BaseClassInfoProvider => _baseClassInfoProvider;

        protected SingleClassGenerator(ICodingUnitInfoProvider classInfoProvider, ICodingUnitInfoProvider baseClassInfoProvider) : base(classInfoProvider)
        {
            _baseClassInfoProvider = baseClassInfoProvider;
        }
        protected override IEnumerable<MemberDeclarationSyntax> PrimaryMemberDeclarations()
        {
            var classDeclarationSyntax = SyntaxFactory.ClassDeclaration(GetClassName())
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            if (CodingUnitInfoProvider.HasBase)
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
            var name = SyntaxFactory.ParseTypeName(_baseClassInfoProvider.Name);
            yield return SyntaxFactory.SimpleBaseType(name);
        }

        protected virtual string GetClassName()
        {
            return CodingUnitInfoProvider.Name;
        }
    }
}
