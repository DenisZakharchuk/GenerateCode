using CodeGeneration.Models.CodingUnits.Meta;
using Microsoft.CodeAnalysis;

namespace CodeGeneration.Services.Base
{
    public interface ICodeGenerator<TCodingUnit> : ICodeGenerator, ICodingUnitService<TCodingUnit>
        where TCodingUnit : CodingUnit
    {

    }
    public interface ICodeGenerator
    {
        SyntaxTree Generate();
    }
}
