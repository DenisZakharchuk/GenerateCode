using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base.Result;
using CodeGeneration.Services.Context;
using CodeGeneration.Services.Naming;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class DeclarationGenerator<TCodingUnit, TResult> : Generator<TCodingUnit, TResult>, IDeclarationGenerator<TCodingUnit, TResult>
        where TResult : class
        where TCodingUnit : CodingUnit
    {

        protected DeclarationGenerator()
        {
        }
    }

    public interface IDeclarationGenerator<TCodingUnit, TResult> : IGenerator<TCodingUnit, TResult>
        where TResult : class
        where TCodingUnit : CodingUnit
    {

    }

    public abstract class CodeGenerator<TCodingUnit> : ICodeGenerator<TCodingUnit>
        where TCodingUnit : CodingUnit
    {
        private TCodingUnit? codingUnit;

        protected virtual TCodingUnit CodingUnit => codingUnit ?? throw new ApplicationException($"{nameof(codingUnit)} is not initiated. Call {nameof(Init)} method first!");
        private readonly IDeclarationProvider declarationProvider;
        private readonly ICodingUnitContextProvider codingUnitContextProvider;
        protected CodeGenerator(IDeclarationProvider declarationProvider, ICodingUnitContextProvider codingUnitContextProvider)
        {
            this.declarationProvider = declarationProvider ?? throw new ArgumentNullException(nameof(declarationProvider));
            this.codingUnitContextProvider = codingUnitContextProvider ?? throw new ArgumentNullException(nameof(codingUnitContextProvider));
            //_codingUnitInfoProvider = codingUnitInfoProvider;
        }
        public virtual GenerationResult Generate(TCodingUnit codingUnit)
        {
            Init(codingUnit);
            return Generate();
        }

        public virtual void Init(TCodingUnit codingUnit)
        {
            this.codingUnit = codingUnit;
            declarationProvider.Init(codingUnit);
            if(codingUnitContextProvider is ICodingUnitContextProvider<TCodingUnit> c)
            {
                c.Init(codingUnit);
            }
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
            return SyntaxFactory.ConstructorDeclaration(declarationProvider.GetName())
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block());
        }
        protected virtual IEnumerable<BaseTypeSyntax> GetBaseTypes()
        {
            if (declarationProvider.HasBase)
            {
                var name = SyntaxFactory.ParseTypeName(declarationProvider.GetBaseName());
                yield return SyntaxFactory.SimpleBaseType(name);
            }
        }
        protected abstract IEnumerable<MemberDeclarationSyntax> GetMembers();
        protected virtual IEnumerable<MemberDeclarationSyntax> PrimaryMemberDeclarations()
        {
            var classDeclarationSyntax = SyntaxFactory.ClassDeclaration(declarationProvider.GetName())
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            if (codingUnitContextProvider.HasBase)
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
            foreach (var ns in codingUnitContextProvider.RequiredNamespaces.OrderBy(x => x))
            {
                compilationUnit = compilationUnit
                    .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(ns)));
            }

            return compilationUnit;
        }
        protected virtual NamespaceDeclarationSyntax NamespaceDeclaration()
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(declarationProvider.GetNamespace()));
        }
    }

    public abstract class CodeGenerator : CodeGenerator<CodingUnit>, ICodeGenerator
    {
        protected CodeGenerator(IDeclarationProvider declarationProvider, ICodingUnitContextProvider codingUnitContextProvider) : base(declarationProvider, codingUnitContextProvider)
        {
        }
    }
}
