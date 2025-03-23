using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base.Result;
using CodeGeneration.Services.Context;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    
    public class TypeParameterConstraintGenerator<TTypeParameter> : Generator<TTypeParameter, TypeParameterConstraintClauseSyntax>, ITypeParameterConstraintGenerator<TTypeParameter>
        where TTypeParameter : Class
    {
        private readonly IClassContextProvider<TTypeParameter> _codingUnitContextProvider;
        public TypeParameterConstraintGenerator(IClassContextProvider<TTypeParameter> codingUnitContextProvider)
        {
            _codingUnitContextProvider = codingUnitContextProvider;
        }
        public override GenerationResult<TypeParameterConstraintClauseSyntax> Generate()
        {
            _codingUnitContextProvider.Init(CodingUnit);

            if (!CodingUnit.IsPrimitive)
            {
                if (_codingUnitContextProvider.HasBase)
                {
                    return GenerationResult.From(SyntaxFactory.TypeParameterConstraintClause(CodingUnit.Name)
                        .AddConstraints(SyntaxFactory.TypeConstraint(SyntaxFactory.ParseTypeName(CodingUnit.BaseModel.Name))));
                }
                else
                {
                    return GenerationResult.From(SyntaxFactory.TypeParameterConstraintClause(CodingUnit.Name)
                        .AddConstraints(SyntaxFactory.ClassOrStructConstraint(SyntaxKind.ClassKeyword)));
                }
            }
            return GenerationResult.From(default(TypeParameterConstraintClauseSyntax));
        }
    }
    public interface ITypeParameterConstraintGenerator<TTypeParameter> : IGenerator<TTypeParameter, TypeParameterConstraintClauseSyntax>, ICodingUnitService<TTypeParameter>
        where TTypeParameter : CodingUnit
    {

    }

    public class TypeParameterGenerator<TTypeParameter> : Generator<TTypeParameter, TypeParameterSyntax>, ITypeParameterGenerator<TTypeParameter>
        where TTypeParameter : CodingUnit
    {
        public override GenerationResult<TypeParameterSyntax> Generate()
        {
            return GenerationResult.From(SyntaxFactory.TypeParameter(CodingUnit.Name));
        }
    }
    public interface ITypeParameterGenerator<TClass> : IGenerator<TClass, TypeParameterSyntax>, ICodingUnitService<TClass>
        where TClass : CodingUnit
    {

    }
}
