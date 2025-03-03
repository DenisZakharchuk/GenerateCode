using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base.Result;

namespace CodeGeneration.Services.Base.String
{
    public abstract class CodeGenerator<TCodingUnit> : ICodeGenerator<TCodingUnit>
        where TCodingUnit : CodingUnit
    {
        private TCodingUnit? _codingUnit;

        protected virtual TCodingUnit CodingUnit => _codingUnit ?? throw new ApplicationException($"{nameof(CodingUnit)} is not initiated.");
        public abstract GenerationResult Generate();

        public void Init(TCodingUnit codingUnit)
        {
            _codingUnit = codingUnit ?? throw new ArgumentNullException(nameof(codingUnit));
        }
    }
}
