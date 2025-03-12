using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base.Result;

namespace CodeGeneration.Services.Base
{
    public interface ICodeGenerator<TCodingUnit> : ICodingUnitService<TCodingUnit>
        where TCodingUnit : CodingUnit
    {
        GenerationResult Generate();
        GenerationResult Generate(TCodingUnit codingUnit) => Generate();
    }
    public interface ICodeGenerator : ICodeGenerator<CodingUnit>
    {
    }
}
