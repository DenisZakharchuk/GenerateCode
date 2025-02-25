using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base;

namespace CodeGeneration.Services.Context
{
    public interface ICodingUnitContextProvider<TCodingUnit> : ICodingUnitContextProvider, ICodingUnitService<TCodingUnit>
        where TCodingUnit : CodingUnit
    {
    }


    public interface ICodingUnitContextProvider
    {
        IEnumerable<string> RequiredNamespaces { get; }
        bool HasBase { get; }
    }
    public abstract class CodingUnitContextProvider<TCodingUnit> : CodingUnitContextProvider, ICodingUnitContextProvider<TCodingUnit>
        where TCodingUnit : CodingUnit
    {
        private TCodingUnit? _codingUnit;

        protected virtual TCodingUnit CodingUnit { get => _codingUnit ?? throw new ApplicationException($"Coding Unit is not initiated. Call method {nameof(Init)} first!"); }

        public void Init(TCodingUnit codingUnit)
        {
            _codingUnit = codingUnit ?? throw new ArgumentNullException(nameof(codingUnit));
        }
    }
    public abstract class CodingUnitContextProvider : ICodingUnitContextProvider
    {
        public CodingUnitContextProvider()
        {
        }

        public abstract IEnumerable<string> RequiredNamespaces { get; }
        public abstract bool HasBase { get; }
    }
}
