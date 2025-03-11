using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Services.Naming
{
    public abstract class DeclarationProvider : IDeclarationProvider
    {
        private CodingUnit? unit;
        public CodingUnit Unit => unit ?? throw new ApplicationException($"{Unit} is not initialized");

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

        public virtual bool HasBase => Unit is Class classCodingUnit && classCodingUnit.BaseModel != null;

        public string GetBaseName()
        {
            return Unit is Class classCodingUnit
                ? classCodingUnit.BaseModel?.Name ?? throw new ApplicationException($"{Unit.Name} does not have a specified base class")
                : throw new ApplicationException($"{Unit.Name} is not a class");
        }

        public virtual string GetName()
        {
            return Unit.Name;
        }
        public virtual string GetNamespace()
        {
            return new string[] { BaseNamespace, Unit.Namespace ?? DefaultNamespace, GetDescriptor() }.
                Where(s => !string.IsNullOrWhiteSpace(s)).
                Aggregate((h, t) => h + "." + t);
        }

        protected abstract string GetDescriptor();

        public void Init(CodingUnit codingUnit)
        {
            unit = codingUnit ?? throw new ArgumentNullException(nameof(codingUnit));
        }
    }
}
