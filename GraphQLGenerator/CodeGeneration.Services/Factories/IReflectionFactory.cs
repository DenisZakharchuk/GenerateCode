using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Services.Factories
{
    public interface IReflectionFactory
    {
        Model Build(Type target);
    }
}
