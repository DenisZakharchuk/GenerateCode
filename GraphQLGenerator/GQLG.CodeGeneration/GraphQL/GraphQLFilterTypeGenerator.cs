using System;
using Microsoft.CodeAnalysis;
using GQLG.Models.Meta;
using GQLG.CodeGeneration.Base;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GQLG.CodeGeneration.GraphQL
{
    public class GraphQLFilterTypeGenerator : SingleClassGenerator
    {
        public GraphQLFilterTypeGenerator(Func<ClassInfo, string> @namespace) : base(@namespace)
        {
        }

        public override SyntaxTree Generate(ClassInfo classInfo)
        {
            throw new NotImplementedException();
        }

        protected override TypeSyntax GetBaseClass(string typeName)
        {
            throw new NotImplementedException();
        }

        protected override string GetClassName(string type)
        {
            throw new NotImplementedException();
        }
    }
}
