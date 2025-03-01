using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGeneration.Services.Mapping
{
    public static class Types
    {
        public static Dictionary<string, SyntaxKind> Map = new Dictionary<string, SyntaxKind>() {
            { "Integer", SyntaxKind.IntKeyword },
            { "String", SyntaxKind.StringKeyword }
        };

        public static SyntaxKind GetPrimitiveType(CodingUnit codingUnit)
        {
            if (codingUnit is null)
            {
                throw new ArgumentNullException(nameof(codingUnit));
            }

            string key = codingUnit.Name;


            if (codingUnit is PropertyInfo propertyInfo)
            {
                key = propertyInfo.Type.Name;
            }
            if (codingUnit is MethodInfo methodInfo)
            {
                key = methodInfo.Type.Name;
            }

            return Map.ContainsKey(key)
                ? Map[key]
                : throw new ApplicationException($"Cannot provide the property type for {codingUnit.Name}");
        }
    }
}
