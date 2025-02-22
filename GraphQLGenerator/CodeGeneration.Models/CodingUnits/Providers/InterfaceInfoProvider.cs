using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Providers.Naming;

namespace CodeGeneration.Models.CodingUnits.Providers
{
    public class InterfaceInfoProvider : CodingUnitInfoProvider<Interface>, IInterfaceInfoProvider
    {
        public InterfaceInfoProvider(Interface codingUnit, INamingProvider namingProvider) : base(codingUnit, namingProvider)
        {
        }
    }
}
