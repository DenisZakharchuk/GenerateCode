using System;
using GQLG.Models.Meta;
using GQLG.CodeGeneration.Base;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GQLG.CodeGeneration.GraphQL
{
    public class GraphQLOrderTypeGenerator : SingleClassGenerator
    {
        public GraphQLOrderTypeGenerator(Func<ClassInfo, string> @namespace) : base(@namespace)
        {
        }

        protected override TypeSyntax GetBaseClass(ClassInfo classInfo)
        {
            throw new NotImplementedException();
        }

        protected override string GetClassName(string type)
        {
            throw new NotImplementedException();
        }
    }
}
