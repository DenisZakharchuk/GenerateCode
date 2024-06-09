using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;
using System;

namespace GQLG.CodeGeneration.Base
{
    public abstract class CodeGenerator
    {
        public abstract SyntaxTree Generate(PropertyInfo[] properties, string typeName);
        public virtual string SubDir() {
            return "GraphQL";
        }

        public virtual string CodeKind()
        {
            return "GraphQLType";
        }
    }
}
