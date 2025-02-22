namespace CodeGeneration.Models.CodingUnits.Providers.Naming
{
    public class DefaultNamingProvider : NamingProvider, INamingProvider
    {
        public DefaultNamingProvider(string baseNamespace, string defaultNamespace) : base(baseNamespace, defaultNamespace)
        {
        }

        protected override string GetDescriptor()
        {
            return "";
        }
    }
}
