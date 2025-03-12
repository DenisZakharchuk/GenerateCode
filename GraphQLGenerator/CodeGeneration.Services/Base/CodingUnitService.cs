using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base.Result;

namespace CodeGeneration.Services.Base
{
    public abstract class Generator<TCodingUnit, TResult> : CodingUnitService<TCodingUnit>, IGenerator<TCodingUnit, TResult>
        where TCodingUnit : CodingUnit
        where TResult : class
    {
        public abstract GenerationResult<TResult> Generate();
    }

    public interface IGenerator<TCodingUnit, TResult> : ICodingUnitService<TCodingUnit>
        where TCodingUnit : CodingUnit
        where TResult : class
    {
        GenerationResult<TResult> Generate();
    }

    public abstract class CodingUnitService<TCodingUnit> : ICodingUnitService<TCodingUnit>
        where TCodingUnit : CodingUnit
    {
        private TCodingUnit? codingUnit;

        protected virtual TCodingUnit CodingUnit => codingUnit ?? throw new ApplicationException($"{nameof(codingUnit)} is not initiated. Call {nameof(ICodingUnitService<TCodingUnit>.Init)} method first!");

        void ICodingUnitService<TCodingUnit>.Init(TCodingUnit codingUnit)
        {
            this.codingUnit = codingUnit ?? throw new ArgumentNullException(nameof(codingUnit));
        }
    }
}
