namespace CodeGeneration.Services.Naming
{
    public class ServiceDeclarationProvider : DeclarationProvider, IServiceDeclarationProvider
    {
        public ServiceDeclarationProvider(string baseNamespace, string defaultNamespace, string serviceName) : base(baseNamespace, defaultNamespace)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; }

        protected override string GetDescriptor()
        {
            return ServiceName;
        }

        public override string GetName()
        {
            return $"{base.GetName()}{ServiceName}";
        }
    }

    public interface IServiceDeclarationProvider : IDeclarationProvider
    {
        string ServiceName { get; }
    }
}
