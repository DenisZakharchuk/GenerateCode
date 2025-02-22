using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.Factories
{
    public interface IReflectionFactory
    {
        CodingUnit Build(Type target);
    }
}
