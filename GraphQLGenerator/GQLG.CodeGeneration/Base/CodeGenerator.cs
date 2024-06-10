using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace GQLG.CodeGeneration.Base
{
    public abstract class CodeGenerator
    {
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
    }
}
