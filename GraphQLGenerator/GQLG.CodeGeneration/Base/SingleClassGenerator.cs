using GQLG.Models.Meta;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

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
        protected abstract TypeSyntax GetBaseClass(string baseTypeName);

        protected virtual ClassDeclarationSyntax ClassDeclaration(ClassInfo classInfo)
        {
            var graphQLTypeName = GetClassName(classInfo.Name);
            var generatedClassBaseType = GetBaseClass(classInfo.Name);

            return SyntaxFactory.ClassDeclaration(graphQLTypeName)
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .AddBaseListTypes(
                                SyntaxFactory.SimpleBaseType(generatedClassBaseType));
        }
    }
}