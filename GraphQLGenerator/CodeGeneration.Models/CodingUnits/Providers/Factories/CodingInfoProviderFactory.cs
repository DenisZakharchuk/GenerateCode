using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Providers.Naming;
using Microsoft.Extensions.DependencyInjection;

namespace CodeGeneration.Models.CodingUnits.Providers.Factories
{
    public class CodingInfoProviderFactory : ICodingInfoProviderFactory
    {
        public IServiceProvider ServiceProvider { get; }

        public CodingInfoProviderFactory(IKeyedServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public bool TryCreateCodingUnitInfoProvider(CodingUnit codingUnit, string? serviceName, out ICodingUnitInfoProvider provider)
        {
            provider = default;
            if(ServiceProvider.GetKeyedServices(typeof(INamingProvider), serviceName) is INamingProvider namingProvider)
            {
                provider = CreateInstance(codingUnit, namingProvider);
                return true;
            }

            return false;
        }

        protected virtual ICodingUnitInfoProvider CreateInstance(CodingUnit codingUnit, INamingProvider namingProvider)
        {
            if (codingUnit is Model model)
            {
                return new ModelInfoProvider(model, namingProvider);
            }

            throw new ApplicationException("Unable to create provider");
        }

        public ICodingUnitInfoProvider CreateCodingUnitInfoProvider(CodingUnit codingUnit, string? serviceName)
        {
            return TryCreateCodingUnitInfoProvider(
                codingUnit, 
                serviceName, 
                out ICodingUnitInfoProvider provider) 
                    ? provider 
                    : throw new ApplicationException("Unable to create provider");
        }
    }

    public interface ICodingInfoProviderFactory
    {
        ICodingUnitInfoProvider CreateCodingUnitInfoProvider(CodingUnit codingUnit, string? serviceName);
        bool TryCreateCodingUnitInfoProvider(CodingUnit codingUnit, string? serviceName, out ICodingUnitInfoProvider provider);
    }
}
