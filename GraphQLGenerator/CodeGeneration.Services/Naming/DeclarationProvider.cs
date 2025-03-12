using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base;

namespace CodeGeneration.Services.Naming
{
    public abstract class DeclarationProvider : CodingUnitService<CodingUnit>, IDeclarationProvider
    {
        protected DeclarationProvider() : this("Base", "Generated")
        {
        }

        protected DeclarationProvider(string baseNamespace, string defaultNamespace)
        {
            BaseNamespace = baseNamespace;
            DefaultNamespace = defaultNamespace;
        }

        protected string DefaultNamespace { get; private set; }

        protected string BaseNamespace { get; private set; }

        public virtual bool HasBase => CodingUnit is Class classCodingUnit && classCodingUnit.BaseModel != null;

        public string GetBaseName()
        {
            return CodingUnit is Class classCodingUnit
                ? classCodingUnit.BaseModel?.Name ?? throw new ApplicationException($"{CodingUnit.Name} does not have a specified base class")
                : throw new ApplicationException($"{CodingUnit.Name} is not a class");
        }

        public virtual string GetName()
        {
            return CodingUnit.Name;
        }
        public virtual string GetNamespace()
        {
            return new string[] { BaseNamespace, CodingUnit.Namespace ?? DefaultNamespace, GetDescriptor() }.
                Where(s => !string.IsNullOrWhiteSpace(s)).
                Aggregate((h, t) => h + "." + t);
        }

        protected abstract string GetDescriptor();
    }
}
