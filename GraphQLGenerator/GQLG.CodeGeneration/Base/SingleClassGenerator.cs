using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GQLG.CodeGeneration.Base
{
    public abstract class SingleClassGenerator : CodeGenerator
    {
        protected SingleClassGenerator(ClassInfo classInfo, Func<ClassInfo, string> @namespace) : base(classInfo, @namespace)
        {
        }

        public abstract string GetClassName(string type);
        protected abstract TypeSyntax GetBaseClass(ClassInfo classInfo);

        protected virtual ClassDeclarationSyntax ClassDeclaration(ClassInfo classInfo)
        {
            var graphQLTypeName = GetClassName(classInfo.Name);

            return SyntaxFactory.ClassDeclaration(graphQLTypeName)
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .AddBaseListTypes(
                                GetBaseListTypes(classInfo))
                .AddMembers(GetMembers(classInfo));
        }

        public virtual MemberDeclarationSyntax[] GetMembers(ClassInfo classInfo)
        {
            return new[] { CreateConstructor(classInfo.Properties, GetClassName(classInfo.Name)) };
        }

        protected virtual BaseTypeSyntax[] GetBaseListTypes(ClassInfo classInfo)
        {
            return new[] { SyntaxFactory.SimpleBaseType(GetBaseClass(classInfo)) };
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
            yield return "MijDim.Web.GraphQL.Types";
            
            if (!string.IsNullOrWhiteSpace(ClassInfo.Namespace))
            {
                yield return ClassInfo.Namespace;
            }
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
                .AddMembers(ClassDeclaration(classInfo)))
                .NormalizeWhitespace();

            return SyntaxFactory.SyntaxTree(compilationUnit, encoding: System.Text.Encoding.UTF8);
        }
    }
}