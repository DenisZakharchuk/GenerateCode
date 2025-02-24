namespace CodeGeneration.Services.Naming
{
    public class DTONamingProvider : NamingProvider, INamingProvider
    {
        public DTONamingProvider(string baseNamespace, string defaultNamespace) : base(baseNamespace, defaultNamespace)
        {
        }

        protected override string GetDescriptor()
        {
            return "DTO";
        }
    }
}
