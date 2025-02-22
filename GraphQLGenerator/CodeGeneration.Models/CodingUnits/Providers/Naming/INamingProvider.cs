using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers.Naming
{
    public interface INamingProvider
    {
        string GetName(CodingUnit unit);
        string GetNamespace(CodingUnit unit);
    }
}
