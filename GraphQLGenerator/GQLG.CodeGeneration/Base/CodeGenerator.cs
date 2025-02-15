using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

//namespace CodeGeneration.Base
//{
//    public abstract class CodeGenerator
//    {
//        private readonly IClassInfoProvider _classInfoProvider;

//        protected CodeGenerator(IClassInfoProvider classInfoProvider)
//        {
//            _classInfoProvider = classInfoProvider;
//        }

//        public abstract SyntaxTree Generate(ClassInfo classInfo);
        
//        protected virtual NamespaceDeclarationSyntax NamespaceDeclaration(ClassInfo classInfo)
//        {
//            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(classInfoProvider.Namespace()));
//        }
//    }
//}
namespace GQLG.CodeGeneration.Base
{
    public abstract class CodeGenerator
    {
        public virtual Func<ClassInfo, string> Namespace { get; set; }
        protected virtual ClassInfo ClassInfo { get; private set; }

        protected CodeGenerator(ClassInfo classInfo, Func<ClassInfo, string> @namespace)
        {
            Namespace = @namespace;
            ClassInfo = classInfo;
        }

        protected CodeGenerator(ClassInfo classInfo) : this(classInfo, c => "Generated")
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

        public virtual string GetClassFileName()
        {
            return $"{ClassInfo.Name}{CodeKind()}.cs";
        }
    }
}
