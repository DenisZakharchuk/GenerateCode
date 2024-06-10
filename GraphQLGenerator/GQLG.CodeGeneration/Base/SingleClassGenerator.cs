using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace GQLG.CodeGeneration.Base
{
    public abstract class SingleClassGenerator : CodeGenerator
    {
        protected SingleClassGenerator(Func<ClassInfo, string> @namespace)
        {
            Namespace = @namespace;
        }
        public virtual Func<ClassInfo, string> Namespace { get; set; }
        protected abstract string GetClassName(string type);
        protected abstract TypeSyntax GetBaseClass(ClassInfo classInfo);
        protected virtual NamespaceDeclarationSyntax NamespaceDeclaration(ClassInfo classInfo)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(Namespace(classInfo)));
        }
        protected virtual ClassDeclarationSyntax ClassDeclaration(ClassInfo classInfo)
        {
            var graphQLTypeName = GetClassName(classInfo.Name);
            var generatedClassBaseType = GetBaseClass(classInfo);

            return SyntaxFactory.ClassDeclaration(graphQLTypeName)
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .AddBaseListTypes(
                                SyntaxFactory.SimpleBaseType(generatedClassBaseType))
                .AddMembers(CreateConstructor(classInfo.Properties, GetClassName(classInfo.Name))); ;
        }
        protected virtual ConstructorDeclarationSyntax CreateConstructor(PropertyInfo[] properties, string graphQLTypeName)
        {
            return SyntaxFactory.ConstructorDeclaration(graphQLTypeName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block());
        }

        public override IEnumerable<string> RequiredNamespaces()
        {
            yield return "System";
            yield return "GraphQL.Types";
        }

        public override SyntaxTree Generate(ClassInfo classInfo)
        {
            var compilationUnit = SyntaxFactory.CompilationUnit();
            foreach (var ns in RequiredNamespaces())
            {
                compilationUnit = compilationUnit
                            .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(ns)));
            }

            compilationUnit = compilationUnit
                .AddMembers(NamespaceDeclaration(classInfo)
                .AddMembers(ClassDeclaration(classInfo)))
                .NormalizeWhitespace();

            return SyntaxFactory.SyntaxTree(compilationUnit, encoding: System.Text.Encoding.UTF8);
        }
    }
}