using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;

namespace GQLG.CodeGeneration.Base
{
    public abstract class CodeGenerator
    {
        public abstract SyntaxTree Generate(PropertyInfo[] properties, string typeName);
    }
}
