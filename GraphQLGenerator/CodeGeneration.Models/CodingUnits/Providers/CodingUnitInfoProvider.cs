using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    public abstract class CodingUnitInfoProvider : ICodingUnitInfoProvider
    {
        private readonly CodingUnit codingUnit;

        protected CodingUnitInfoProvider(CodingUnit codingUnit)
        {
            this.codingUnit = codingUnit ?? throw new ArgumentNullException(nameof(codingUnit));
        }

        public abstract string Name { get; }
        public abstract string? Namespace { get; }
        public abstract IEnumerable<string> RequiredNamespaces { get; }
        public abstract bool HasBase { get; }

        protected CodingUnit CodingUnit => codingUnit;
    }
}
