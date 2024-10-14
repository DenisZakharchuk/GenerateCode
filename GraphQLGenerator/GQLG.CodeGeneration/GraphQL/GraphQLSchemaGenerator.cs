using System;
using GQLG.CodeGeneration.Base;
using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;

namespace GQLG
{
    public class GraphQLSchemaGenerator : CodeGenerator
    {
        public GraphQLSchemaGenerator(ClassInfo classInfo, Func<ClassInfo, string> @namespace) : base(classInfo, @namespace)
        {
        }

        public override SyntaxTree Generate(ClassInfo classInfo)
        {
            throw new NotImplementedException();
        }
    }
}
