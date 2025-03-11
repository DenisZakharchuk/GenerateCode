namespace CodeGeneration.Services.Naming
{
    public class DefaultDeclarationProvider : DeclarationProvider, IDefaultDeclarationProvider
    {
        public DefaultDeclarationProvider(string baseNamespace, string defaultNamespace) : base(baseNamespace, defaultNamespace)
        {
        }

        protected override string GetDescriptor()
        {
            return "";
        }
    }

    public interface IDefaultDeclarationProvider : IDeclarationProvider
    {
    }
}
