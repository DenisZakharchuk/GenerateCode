using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Providers.Naming;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    public abstract class CodingUnitInfoProvider<TCodingUnit> : ICodingUnitInfoProvider
        where TCodingUnit: CodingUnit
    {
        private readonly TCodingUnit codingUnit;
        private readonly INamingProvider namingProvider;

        protected CodingUnitInfoProvider(TCodingUnit codingUnit, INamingProvider namingProvider)
        {
            this.codingUnit = codingUnit ?? throw new ArgumentNullException(nameof(codingUnit));
            this.namingProvider = namingProvider ?? throw new ArgumentNullException(nameof(namingProvider));
        }

        public string Name => namingProvider.GetName(codingUnit);
        public string? Namespace => namingProvider.GetNamespace(codingUnit);
        public virtual IEnumerable<string> RequiredNamespaces => [];
        public virtual bool HasBase => false;

        protected TCodingUnit CodingUnit => codingUnit;
    }
}
