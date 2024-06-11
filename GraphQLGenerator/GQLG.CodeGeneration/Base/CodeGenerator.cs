using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace GQLG.CodeGeneration.Base
{
    public abstract class CodeGenerator
    {
        public virtual Func<ClassInfo, string> Namespace { get; set; }

        protected CodeGenerator(Func<ClassInfo, string> @namespace)
        {
            Namespace = @namespace;
        }

        protected CodeGenerator() : this(c => "Generated")
        {
        }

        public abstract SyntaxTree Generate(ClassInfo classInfo);
        public virtual string SubDir() 
        {
            return "GraphQL";
        }

        public virtual string CodeKind()
        {
            return "GraphQLType";
        }

        public virtual IEnumerable<string> RequiredNamespaces()
        {
            yield return "System";
        }
        protected virtual NamespaceDeclarationSyntax NamespaceDeclaration(ClassInfo classInfo)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(Namespace(classInfo)));
        }
    }
}
