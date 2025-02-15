using CodeGeneration.Models.CodingUnits.Providers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class CodeGenerator
    {
        private readonly ICodingUnitInfoProvider _codingUnitInfoProvider;

        protected ICodingUnitInfoProvider CodingUnitInfoProvider => _codingUnitInfoProvider;

        protected CodeGenerator(ICodingUnitInfoProvider codingUnitInfoProvider)
        {
            _codingUnitInfoProvider = codingUnitInfoProvider;
        }

        public SyntaxTree Generate()
        {
            var compilationUnit = SyntaxFactory.CompilationUnit();
            compilationUnit = AppendUsings(compilationUnit);

            compilationUnit = compilationUnit
                .AddMembers(NamespaceDeclaration()
                    .AddMembers(PrimaryMemberDeclarations().ToArray()))
                .NormalizeWhitespace();

            return SyntaxFactory.SyntaxTree(compilationUnit, encoding: System.Text.Encoding.UTF8);
        }

        protected abstract IEnumerable<MemberDeclarationSyntax> PrimaryMemberDeclarations();


        protected virtual CompilationUnitSyntax AppendUsings(CompilationUnitSyntax compilationUnit)
        {
            foreach (var ns in _codingUnitInfoProvider.RequiredNamespaces.OrderBy(x => x))
            {
                compilationUnit = compilationUnit
                            .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(ns)));
            }

            return compilationUnit;
        }

        protected virtual NamespaceDeclarationSyntax NamespaceDeclaration()
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(_codingUnitInfoProvider.Namespace));
        }
    }

}
