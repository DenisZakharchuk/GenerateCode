using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.Factories
{
    public interface IReflectionFactory
    {
        ClassInfo Build(Type target);
    }
}
