namespace CodeGeneration.Services.Naming
{
    public class DTODeclarationProvider : DeclarationProvider, IDeclarationProvider
    {
        public DTODeclarationProvider(string baseNamespace, string defaultNamespace) : base(baseNamespace, defaultNamespace)
        {
        }

        protected override string GetDescriptor()
        {
            return "DTO";
        }
    }
}
