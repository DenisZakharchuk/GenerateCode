using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    public class ClassInfoProvider : CodingUnitInfoProvider, IClassInfoProvider
    {
        public ClassInfoProvider(CodingUnit info) : base(info)
        {
        }
    }
}
