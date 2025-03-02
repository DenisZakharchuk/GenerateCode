using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    public abstract class CodingUnitInfoProvider<TCodingUnit> : ICodingUnitInfoProvider
        where TCodingUnit: CodingUnit
    {
        private readonly TCodingUnit codingUnit;        

        protected CodingUnitInfoProvider(TCodingUnit codingUnit)
        {
            this.codingUnit = codingUnit ?? throw new ArgumentNullException(nameof(codingUnit));
            Name = codingUnit.Name;
            //Namespace = codingUnit.Namespace;
            RequiredNamespaces = [];
        }

        public string Name { get; }
        public string? Namespace { get; }
        public virtual IEnumerable<string> RequiredNamespaces { get; }
        public virtual bool HasBase { get; }

        protected TCodingUnit CodingUnit => codingUnit;
    }
}
