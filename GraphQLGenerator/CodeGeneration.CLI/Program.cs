using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Providers;
using CodeGeneration.Models.CodingUnits.Providers.Default;
using CodeGeneration.Models.CodingUnits.Providers.Factories;
using CodeGeneration.Models.CodingUnits.Providers.Naming;
using CodeGeneration.Services.Generators;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static void Main(string[] args)
    {
        const string BaseNamespace = "DZAKH";
        const string DefaultNamespace = "Test";

        CodingUnit[] models = [
            new Model() {
                Name = "Customer",
                Namespace = "System",
                Properties = [
                    new PropertyInfo() {
                        Name = "FullName",
                        Type = "String"
                    },
                    new PropertyInfo()
                    {
                        Name = "Email",
                        Type = "String"
                    },
                    new PropertyInfo()
                    {
                        Name = "Password",
                        Type = "String"
                    }
                ]
            },
            new Model(){
                Name = "Device",
                Namespace = "System",
                Properties = [
                    new PropertyInfo(){
                        Name = "Name",
                        Type = "String"
                    },
                    new PropertyInfo(){
                        Name = "Validity",
                        Type = "String"
                    },
                    new PropertyInfo(){
                        Name = "QQ",
                        Type = "String"
                    },
                ]
            }
        ];


        string[] serviceKeys = ["Reader", "Facade", "Updater", "Validator"];

        IServiceCollection servicesCollection = new ServiceCollection();

        servicesCollection.AddTransient<ICodingInfoProviderFactory, CodingInfoProviderFactory>();

        foreach (var serviceKey in serviceKeys)
        {
            servicesCollection.AddKeyedTransient<INamingProvider>(
                serviceKey,
                (sp, key) => new ServiceNamingProvider(BaseNamespace, DefaultNamespace, key?.ToString() ?? "Service")
            );
        }

        servicesCollection.AddTransient<IDefaultNamingProvider>(sp => new DefaultNamingProvider(BaseNamespace, DefaultNamespace));

        var serviceProvider = servicesCollection.BuildServiceProvider();

        var factory = serviceProvider.GetRequiredService<ICodingInfoProviderFactory>();

        foreach (var modelInfo in models)
        {
            foreach (var key in serviceKeys)
            {
                var provider = factory.CreateCodingUnitInfoProvider(modelInfo, key);

            }
        }


        foreach (var provider in providers)
        {
            ServiceClassGenerator serviceClassGenerator = new(provider, new DefaultBaseClassInfoProvider());

        }
    }
}