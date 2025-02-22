using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Models.CodingUnits.Providers.Naming
{
    public class ServiceNamingProvider : NamingProvider, IServiceNamingProvider
    {
        public ServiceNamingProvider(string baseNamespace, string defaultNamespace, string serviceName) : base(baseNamespace, defaultNamespace)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; }

        protected override string GetDescriptor()
        {
            return ServiceName;
        }

        public override string GetName(CodingUnit unit)
        {
            return $"{base.GetName(unit)}{ServiceName}";
        }
    }

    public interface IServiceNamingProvider : INamingProvider
    {
        string ServiceName { get; }
    }
}
