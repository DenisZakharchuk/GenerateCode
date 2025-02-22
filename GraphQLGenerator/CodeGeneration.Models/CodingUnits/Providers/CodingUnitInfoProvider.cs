using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    public abstract class CodingUnitInfoProvider : ICodingUnitInfoProvider
    {
        protected CodingUnitInfoProvider(CodingUnit codingUnit)
        {
            Name = codingUnit.Name;
            Namespace = codingUnit.Namespace;
            HasBase = false;
            RequiredNamespaces = [];
        }

        public string Name { get; }
        public string? Namespace { get; }
        public IEnumerable<string> RequiredNamespaces { get; }
        public bool HasBase { get; }
    }
}
