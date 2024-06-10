using System;
using GQLG.CodeGeneration.Base;
using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;

namespace GQLG
{
    public class GraphQLQueryGenerator : CodeGenerator
    {
        public override SyntaxTree Generate(PropertyInfo[] properties, string typeName)
        {
            throw new NotImplementedException();
        }

        public override SyntaxTree Generate(ClassInfo classInfo)
        {
            throw new NotImplementedException();
        }
    }
}
