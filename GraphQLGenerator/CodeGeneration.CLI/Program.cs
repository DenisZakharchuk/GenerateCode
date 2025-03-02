using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Naming;
using CodeGeneration.Services.Generators;
using Microsoft.Extensions.DependencyInjection;
using CodeGeneration.Services.Context;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base;

internal class Program
{
    private static void Main(string[] args)
    {
        var _string = new Class() { Name = "String", Namespace = "System" };

        const string BaseNamespace = "TBI.FOS";
        const string DefaultNamespace = "Test";

        Model[] models = [
            new Model() {
                Name = "Customer",
                Namespace = "Models",
                Properties = [
                    new PropertyInfo() {
                        Name = "FullName",
                        //Type = _string.Name,
                        Type = _string
                    },
                    new PropertyInfo()
                    {
                        Name = "Email",
                        Type = _string,
                        //PropertyType = _string
                    },
                    new PropertyInfo()
                    {
                        Name = "Password",
                        Type = _string,
                        //PropertyType = _string
                    }
                ],
                Methods = [
                    new MethodInfo(){
                        Name = "SetFullName",
                        Type = _string
                    },
                    new MethodInfo(){
                        Name = "SetPassword",
                        Type = _string,
                    },
                    new MethodInfo(){
                        Name = "SetEmail",
                        Type = _string
                    }
                ]
            },
            new Model(){
                Name = "Device",
                Namespace = "Models",
                Properties = [
                    new PropertyInfo(){
                        Name = "Name",
                        Type = _string
                    },
                    new PropertyInfo(){
                        Name = "Validity",
                        Type = _string
                    },
                    new PropertyInfo(){
                        Name = "QQ",
                        Type = _string
                    },
                ]
            }
        ];

        string[] serviceKeys = ["Reader", "Facade", "Updater", "Validator"];

        IServiceCollection servicesCollection = new ServiceCollection();

        servicesCollection.AddTransient<IServiceNamingProvider, ServiceNamingProvider>();
        servicesCollection.AddTransient<IDefaultNamingProvider>(sp => new DefaultNamingProvider(BaseNamespace, DefaultNamespace));

        servicesCollection.AddTransient<IModelContextProvider, ModelContextProvider>();
        servicesCollection.AddTransient<IBehaviourContextProvider, BehaviourContextProvider>();

        servicesCollection.AddTransient<IMemberGenerator<PropertyInfo>, PropertyGenerator>();
        servicesCollection.AddTransient<IMemberGenerator<MethodInfo>, MethodGenerator>();

        servicesCollection.AddTransient<IServiceClassGenerator, ServiceClassGenerator>();
        servicesCollection.AddTransient<IModelGenerator, ModelGenerator>();

        var serviceProvider = servicesCollection.BuildServiceProvider();

        //var factory = serviceProvider.GetRequiredService<ICodingInfoProviderFactory>();

        var generator = serviceProvider.GetRequiredService<IModelGenerator>();

        string outputFolder = "C:\\test\\generation";

        foreach (var modelInfo in models)
        {
            var result = generator.Generate(modelInfo);
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