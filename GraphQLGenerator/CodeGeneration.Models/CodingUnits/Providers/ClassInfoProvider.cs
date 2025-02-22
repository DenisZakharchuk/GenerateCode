using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    
    public class ClassInfoProvider : CodingUnitInfoProvider, IClassInfoProvider
    {
        public ClassInfoProvider(CodingUnit info) : base(info)
        {
        }

        public override string Name => throw new NotImplementedException();

        public override string? Namespace => throw new NotImplementedException();

        public override IEnumerable<string> RequiredNamespaces => throw new NotImplementedException();

        public override bool HasBase => throw new NotImplementedException();
    }
}
