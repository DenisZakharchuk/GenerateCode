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

        servicesCollection.AddTransient<IServiceDeclarationProvider, ServiceDeclarationProvider>();
        servicesCollection.AddTransient<IDefaultDeclarationProvider>(sp => new DefaultDeclarationProvider(BaseNamespace, DefaultNamespace));

        servicesCollection.AddTransient<IModelContextProvider, ModelContextProvider>();
        servicesCollection.AddTransient<IBehaviourContextProvider, BehaviourContextProvider>();

        servicesCollection.AddTransient<IMemberGenerator<PropertyInfo>, PropertyGenerator>();
        servicesCollection.AddTransient<IMemberGenerator<MethodInfo>, MethodGenerator>();

        servicesCollection.AddTransient<IServiceClassGenerator, ServiceClassGenerator>();
        servicesCollection.AddTransient<IModelGenerator, ModelGenerator>();

        servicesCollection.AddTransient<CodeGeneration.Services.Generators.TypeScriptGenerators.IModelGenerator, CodeGeneration.Services.Generators.TypeScriptGenerators.ModelGenerator>();

        servicesCollection.AddTransient<CodeGeneration.Services.Generators.TypeScriptGenerators.ITypeScriptPropertyGenerator, CodeGeneration.Services.Generators.TypeScriptGenerators.TypeScriptPropertyGenerator>();
        servicesCollection.AddTransient<CodeGeneration.Services.Generators.TypeScriptGenerators.ITypeScriptMethodGenerator, CodeGeneration.Services.Generators.TypeScriptGenerators.TypeScriptMethodGenerator>();

        servicesCollection.AddTransient<CodeGeneration.Services.Generators.TypeScriptGenerators.ITypeScriptClassTemplate, CodeGeneration.Services.Generators.TypeScriptGenerators.TypeScriptClassTemplate>();
        servicesCollection.AddTransient<CodeGeneration.Services.Generators.TypeScriptGenerators.ITypeScriptMethodTemplate, CodeGeneration.Services.Generators.TypeScriptGenerators.TypeScriptMethodTemplate>();
        servicesCollection.AddTransient<CodeGeneration.Services.Generators.TypeScriptGenerators.ITypeScriptPropertyTemplate, CodeGeneration.Services.Generators.TypeScriptGenerators.TypeScriptPropertyTemplate>();
        

        var serviceProvider = servicesCollection.BuildServiceProvider();

        //var factory = serviceProvider.GetRequiredService<ICodingInfoProviderFactory>();

        var generator = serviceProvider.GetRequiredService<CodeGeneration.Services.Generators.TypeScriptGenerators.IModelGenerator>();

        string outputFolder = "C:\\test\\generation\\ts";

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