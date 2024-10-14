using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace GQLG.CodeGeneration.Base
{
    public abstract class SingleInterfaceGenerator : CodeGenerator
    {
        protected SingleInterfaceGenerator(ClassInfo classInfo, Func<ClassInfo, string> @namespace) : base(classInfo, @namespace)
        {
        }

        public override SyntaxTree Generate(ClassInfo classInfo)
        {
            var compilationUnit = SyntaxFactory.CompilationUnit();
            foreach (var ns in RequiredNamespaces().OrderBy(x => x))
            {
                compilationUnit = compilationUnit
                            .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(ns)));
            }

            compilationUnit = compilationUnit
                .AddMembers(NamespaceDeclaration(classInfo)
                .AddMembers(InterfaceDeclaration(classInfo)))
                .NormalizeWhitespace();

            return SyntaxFactory.SyntaxTree(compilationUnit, encoding: System.Text.Encoding.UTF8);
        }
        protected virtual InterfaceDeclarationSyntax InterfaceDeclaration(ClassInfo classInfo)
        {
            var graphQLTypeName = GetInterfaceName(classInfo.Name);
            var generatedBaseType = GetBaseInterface(classInfo);

            return SyntaxFactory.InterfaceDeclaration(graphQLTypeName)
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .AddBaseListTypes(
                                SyntaxFactory.SimpleBaseType(generatedBaseType));
        }

        protected abstract string GetInterfaceName(string type);
        protected abstract TypeSyntax GetBaseInterface(ClassInfo classInfo);

    }
}