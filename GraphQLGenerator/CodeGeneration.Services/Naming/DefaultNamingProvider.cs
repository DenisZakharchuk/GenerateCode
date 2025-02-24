namespace CodeGeneration.Services.Naming
{
    public class DefaultNamingProvider : NamingProvider, IDefaultNamingProvider
    {
        public DefaultNamingProvider(string baseNamespace, string defaultNamespace) : base(baseNamespace, defaultNamespace)
        {
        }

        protected override string GetDescriptor()
        {
            return "";
        }
    }

    public interface IDefaultNamingProvider : INamingProvider
    {
    }
}
