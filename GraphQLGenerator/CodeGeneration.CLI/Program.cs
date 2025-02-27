using CodeGeneration.Models.CodingUnits.Meta;

using CodeGeneration.Services.Naming;
using CodeGeneration.Services.Generators;
using Microsoft.Extensions.DependencyInjection;
using CodeGeneration.Services.Context;

internal class Program
{
    private static void Main(string[] args)
    {
        var _string = new CodingUnit() { Name = "String", Namespace = "System" };

        const string BaseNamespace = "TBI.FOS";
        const string DefaultNamespace = "Test";

        Model[] models = [
            new Model() {
                Name = "Customer",
                Namespace = "Models",
                Properties = [
                    new PropertyInfo() {
                        Name = "FullName",
                        Type = _string.Name,
                        PropertyType = _string
                    },
                    new PropertyInfo()
                    {
                        Name = "Email",
                        Type = _string.Name,
                        PropertyType = _string
                    },
                    new PropertyInfo()
                    {
                        Name = "Password",
                        Type = _string.Name,
                        PropertyType = _string
                    }
                ]
            },
            new Model(){
                Name = "Device",
                Namespace = "Models",
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

        //servicesCollection.AddTransient<ICodingInfoProviderFactory, CodingInfoProviderFactory>();

        //foreach (var serviceKey in serviceKeys)
        //{
        //    servicesCollection.AddKeyedTransient<INamingProvider>(
        //        serviceKey,
        //        (sp, key) => new ServiceNamingProvider(BaseNamespace, DefaultNamespace, key?.ToString() ?? "Service")
        //    );
        //}

        servicesCollection.AddTransient<IServiceNamingProvider, ServiceNamingProvider>();
        servicesCollection.AddTransient<IDefaultNamingProvider>(sp => new DefaultNamingProvider(BaseNamespace, DefaultNamespace));

        servicesCollection.AddTransient<IModelContextProvider, ModelContextProvider>();
        servicesCollection.AddTransient<IBehaviourContextProvider, BehaviourContextProvider>();

        servicesCollection.AddTransient<IServiceClassGenerator, ServiceClassGenerator>();
        servicesCollection.AddTransient<IModelGenerator, ModelGenerator>();

        var serviceProvider = servicesCollection.BuildServiceProvider();

        //var factory = serviceProvider.GetRequiredService<ICodingInfoProviderFactory>();

        var generator = serviceProvider.GetRequiredService<IModelGenerator>();

        string outputFolder = "C:\\test\\generation";

        foreach (var modelInfo in models)
        {
            generator.Init(modelInfo);
            var result = generator.Generate();
            if(result != null)
            {
                // Save generated class file
                var outputSubFolder = Path.Combine(outputFolder, modelInfo.Name, modelInfo.Namespace ?? "default");

                // Ensure output directory exists
                Directory.CreateDirectory(outputSubFolder);

                var outputCsPath = Path.Combine(outputSubFolder, $"{modelInfo.Name}.cs");

                File.WriteAllText(outputCsPath, result.ToString());
            }
        }
    }
}