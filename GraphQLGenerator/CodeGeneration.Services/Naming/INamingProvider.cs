using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Services.Naming
{
    public interface INamingProvider
    {
        string GetName(CodingUnit unit);
        string GetNamespace(CodingUnit unit);
    }
}
