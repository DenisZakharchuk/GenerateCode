using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base.Result;
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
        private TCodingUnit? codingUnit;

        protected virtual TCodingUnit CodingUnit => codingUnit ?? throw new ApplicationException($"{nameof(codingUnit)} is not initiated. Call {nameof(Init)} method first!"); 
        
        protected CodeGenerator(IDeclarationProvider namingProvider, ICodingUnitContextProvider<TCodingUnit> codingUnitContextProvider) : base(namingProvider, codingUnitContextProvider)
        {
        }

        public virtual GenerationResult Generate(TCodingUnit codingUnit)
        {
            DeclarationProvider.Init(codingUnit);
            Init(codingUnit);
            return Generate();
        }

        public virtual void Init(TCodingUnit codingUnit)
        {
            this.codingUnit = codingUnit;
            if(CodingUnitContextProvider is ICodingUnitContextProvider<TCodingUnit> c)
            {
                c.Init(codingUnit);
            }
        }
    }
    public abstract class CodeGenerator : ICodeGenerator
    {
        private readonly IDeclarationProvider declarationProvider;
        private readonly ICodingUnitContextProvider codingUnitContextProvider;
        public IDeclarationProvider DeclarationProvider => declarationProvider;
        public ICodingUnitContextProvider CodingUnitContextProvider => codingUnitContextProvider;

        protected CodeGenerator(IDeclarationProvider declarationProvider, ICodingUnitContextProvider codingUnitContextProvider)
        {
            this.declarationProvider = declarationProvider ?? throw new ArgumentNullException(nameof(declarationProvider));
            this.codingUnitContextProvider = codingUnitContextProvider ?? throw new ArgumentNullException(nameof(codingUnitContextProvider));
            //_codingUnitInfoProvider = codingUnitInfoProvider;
        }

        public GenerationResult Generate()
        {
            var compilationUnit = CreateRootCompilationUnit();

            compilationUnit = compilationUnit
                .AddMembers(NamespaceDeclaration()
                    .AddMembers(PrimaryMemberDeclarations().ToArray()))
                .NormalizeWhitespace();

            return new GenerationResult<SyntaxTree>(SyntaxFactory.SyntaxTree(compilationUnit, encoding: System.Text.Encoding.UTF8));
        }
        protected virtual CompilationUnitSyntax CreateRootCompilationUnit()
        {
            var compilationUnit = SyntaxFactory.CompilationUnit();
            compilationUnit = AppendUsings(compilationUnit);
            return compilationUnit;
        }
        protected virtual ConstructorDeclarationSyntax GenerateConstructor()
        {
            return SyntaxFactory.ConstructorDeclaration(DeclarationProvider.GetName())
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block());
        }
        protected virtual IEnumerable<BaseTypeSyntax> GetBaseTypes()
        {
            if (DeclarationProvider.HasBase)
            {
                var name = SyntaxFactory.ParseTypeName(DeclarationProvider.GetBaseName());
                yield return SyntaxFactory.SimpleBaseType(name);
            }
        }
        protected abstract IEnumerable<MemberDeclarationSyntax> GetMembers();
        protected virtual IEnumerable<MemberDeclarationSyntax> PrimaryMemberDeclarations()
        {
            var classDeclarationSyntax = SyntaxFactory.ClassDeclaration(DeclarationProvider.GetName())
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
        protected virtual CompilationUnitSyntax AppendUsings(CompilationUnitSyntax compilationUnit)
        {
            foreach (var ns in CodingUnitContextProvider.RequiredNamespaces.OrderBy(x => x))
            {
                compilationUnit = compilationUnit
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(ns)));
            }

            return compilationUnit;
        }
        protected virtual NamespaceDeclarationSyntax NamespaceDeclaration()
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(DeclarationProvider.GetNamespace()));
        }
    }
}
