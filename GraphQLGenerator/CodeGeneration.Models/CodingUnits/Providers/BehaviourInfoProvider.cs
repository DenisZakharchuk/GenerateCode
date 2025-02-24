using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    public class BehaviourInfoProvider : CodingUnitInfoProvider<Behaviour>, IBehaviourInfoProvider
    {
        public BehaviourInfoProvider(Behaviour codingUnit) : base(codingUnit)
        {
        }
    }
}
