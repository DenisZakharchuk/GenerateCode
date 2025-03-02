using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base.Result;

namespace CodeGeneration.Services.Base
{
    public interface ICodeGenerator<TCodingUnit> : ICodeGenerator, ICodingUnitService<TCodingUnit>
        where TCodingUnit : CodingUnit
    {
        GenerationResult Generate(TCodingUnit codingUnit) => Generate();
    }
    public interface ICodeGenerator
    {
        GenerationResult Generate();
    }
}
