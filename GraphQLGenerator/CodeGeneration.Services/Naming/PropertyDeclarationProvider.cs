using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base;
using CodeGeneration.Services.Base.Result;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Naming
{
    public class PropertyDeclarationProvider : CodingUnitService<PropertyInfo>, IDeclarationGenerator<PropertyInfo, TypeSyntax>
    {
        public GenerationResult<TypeSyntax> Generate()
        {
            return GenerationResult.From(GetPropertyTypeSyntax());
        }

        private TypeSyntax GetPropertyTypeSyntax()
        {
            if (CodingUnit.Type.IsPrimitive)
            {
                return SyntaxFactory.PredefinedType(SyntaxFactory.Token(Mapping.Types.GetPrimitiveType(CodingUnit)));
            }
            else if (CodingUnit.Type.IsCollection)
            {
                if (CodingUnit.Type is Class _class && _class.GenericTypeArguments != null)
                {
                    var itemType = SyntaxFactory.ParseTypeName(_class.GenericTypeArguments.First().Name);

                    var enumerableType = SyntaxFactory.GenericName(nameof(System.Collections.IEnumerable))
                        .AddTypeArgumentListArguments(itemType);

                    return enumerableType;
                }
                else
                {
                    throw new ApplicationException($"{CodingUnit.Type.Name} is collection but does not have GenericTypeArguments");
                }
            } 
            else
            {
                return SyntaxFactory.ParseTypeName(CodingUnit.Type.Name);
            }
        }
    }
}
