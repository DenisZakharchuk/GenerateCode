using System;
using System.Linq;
using GQLG.CodeGeneration.Base;
using GQLG.Models.Meta;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GQLG
{
    public class GraphQLDataReaderGenerator : SingleClassGenerator
    {
        public GraphQLDataReaderGenerator(Func<ClassInfo, string> @namespace) : base(@namespace)
        {
        }

        protected override TypeSyntax GetBaseClass(ClassInfo classInfo)
        {
            return SyntaxFactory.GenericName(SyntaxFactory.Identifier("GraphQLDataReader"))
                .AddTypeArgumentListArguments(SyntaxFactory.ParseTypeName(classInfo.Name));
        }

        protected override BaseTypeSyntax[] GetBaseListTypes(ClassInfo classInfo)
        {
            var baseTypeSyntaxes = base.GetBaseListTypes(classInfo).ToList();
            baseTypeSyntaxes.Add(SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"I{GetClassName(classInfo.Name)}")));
            return baseTypeSyntaxes.ToArray();
        }

        protected override string GetClassName(string type)
        {
            return $"{type}DataReader";
        }
        public override string CodeKind()
        {
            return "DataReader";
        }
    }
}
