using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    public class InterfaceInfoProvider : CodingUnitInfoProvider, IInterfaceInfoProvider
    {
        public InterfaceInfoProvider(CodingUnit codingUnit) : base(codingUnit)
        {
        }
    }
}
