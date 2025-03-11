using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base;

namespace CodeGeneration.Services.Naming
{
    public interface IDeclarationProvider : ICodingUnitService<CodingUnit>
    {
        bool HasBase { get; }

        string GetBaseName();
        string GetName();
        string GetNamespace();
    }
}
