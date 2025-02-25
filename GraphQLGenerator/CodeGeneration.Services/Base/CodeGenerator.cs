using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class CodeGenerator<TCodingUnit> : CodeGenerator, ICodeGenerator<TCodingUnit>
        where TCodingUnit : CodingUnit
    {
        private readonly TCodingUnit? codingUnit;

        protected virtual TCodingUnit CodingUnit => codingUnit ?? throw new ApplicationException($"{nameof(codingUnit)} is not initiated. Call {nameof(Init)} method first!"); 
        
        protected CodeGenerator(INamingProvider namingProvider, ICodingUnitContextProvider<TCodingUnit> codingUnitContextProvider) : base(namingProvider, codingUnitContextProvider)
        {
        }

        public virtual void Init(TCodingUnit codingUnit)
        {
            if(CodingUnitContextProvider is ICodingUnitContextProvider<TCodingUnit> c)
            {
                c.Init(codingUnit);
            }
        }

        protected override NamespaceDeclarationSyntax NamespaceDeclaration()
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(NamingProvider.GetNamespace(CodingUnit)));
        }
    }
    public abstract class CodeGenerator : ICodeGenerator
    {
        private readonly INamingProvider namingProvider;
        private readonly ICodingUnitContextProvider codingUnitContextProvider;

        public INamingProvider NamingProvider => namingProvider;
        public ICodingUnitContextProvider CodingUnitContextProvider => codingUnitContextProvider;


        protected CodeGenerator(INamingProvider namingProvider, ICodingUnitContextProvider codingUnitContextProvider)
        {
            this.namingProvider = namingProvider ?? throw new ArgumentNullException(nameof(namingProvider));
            this.codingUnitContextProvider = codingUnitContextProvider ?? throw new ArgumentNullException(nameof(codingUnitContextProvider));
            //_codingUnitInfoProvider = codingUnitInfoProvider;
        }

        public SyntaxTree Generate()
        {
            var compilationUnit = CreateRootCompilationUnit();

            compilationUnit = compilationUnit
                .AddMembers(NamespaceDeclaration()
                    .AddMembers(PrimaryMemberDeclarations().ToArray()))
                .NormalizeWhitespace();

            return SyntaxFactory.SyntaxTree(compilationUnit, encoding: System.Text.Encoding.UTF8);
        }

        protected virtual CompilationUnitSyntax CreateRootCompilationUnit()
        {
            var compilationUnit = SyntaxFactory.CompilationUnit();
            compilationUnit = AppendUsings(compilationUnit);
            return compilationUnit;
        }

        protected abstract IEnumerable<MemberDeclarationSyntax> PrimaryMemberDeclarations();

        protected virtual CompilationUnitSyntax AppendUsings(CompilationUnitSyntax compilationUnit)
        {
            foreach (var ns in CodingUnitContextProvider.RequiredNamespaces.OrderBy(x => x))
            {
                compilationUnit = compilationUnit
                            .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(ns)));
            }

            return compilationUnit;
        }

        protected abstract NamespaceDeclarationSyntax NamespaceDeclaration();
    }
}
