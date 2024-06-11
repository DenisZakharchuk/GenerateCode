using System;
using GQLG.CodeGeneration.Base;
using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;
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
            throw new NotImplementedException();
        }

        protected override string GetClassName(string type)
        {
            throw new NotImplementedException();
        }
    }
}
