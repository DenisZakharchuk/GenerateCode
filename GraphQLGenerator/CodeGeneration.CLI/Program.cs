using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Naming;
using CodeGeneration.Services.Generators;
using Microsoft.Extensions.DependencyInjection;
using CodeGeneration.Services.Context;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class Program
{
    private static void Main(string[] args)
    {
        var _string = new Class() { Name = "String", Namespace = "System" };
        var _date = new Class() { Name = "DateTime", Namespace = "System" };

        const string BaseNamespace = "TBI.FOS";
        const string DefaultNamespace = "Test";

        var _customer = new Model()
        {
            Name = "Customer",
            Namespace = $"{BaseNamespace}.Models",
            Properties = [
                new PropertyInfo()
                {
                    Name = "CreatedOn",
                    Type = _date,
                    IsPrimitive = false,
                },
                new PropertyInfo()
                {
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
                new MethodInfo()
                {
                    Name = "SetFullName",
                    Type = _string
                },
                new MethodInfo()
                {
                    Name = "SetPassword",
                    Type = _string,
                },
                new MethodInfo()
                {
                    Name = "SetEmail",
                    Type = _string
                }
            ]
        };

        Class _customers = new Class()
        {
            IsCollection = true,
            GenericTypeArguments = [_customer],
            Name = "Customer",
            IsPrimitive = false
        };

        Model[] models = [ _customer,
            new Model()
            {
                Name = "Device",
                Namespace = $"{BaseNamespace}.Models",
                Properties = [
                    new PropertyInfo()
                    {
                        Name = "Customers",
                        Type = _customers,
                        IsCollection = true,
                    },
                    new PropertyInfo()
                    {
                        Name = "Name",
                        Type = _string
                    },
                    new PropertyInfo()
                    {
                        Name = "Validity",
                        Type = _string
                    },
                    new PropertyInfo()
                    {
                        Name = "QQ",
                        Type = _string
                    },
                ]
            }
        ];

        string[] serviceKeys = ["Reader", "Facade", "Updater", "Validator"];

        IServiceCollection servicesCollection = new ServiceCollection();

        servicesCollection.AddTransient<IDeclarationGenerator<PropertyInfo, TypeSyntax>, PropertyDeclarationProvider>();
        servicesCollection.AddTransient<IServiceDeclarationProvider, ServiceDeclarationProvider>();
        servicesCollection.AddTransient<IDefaultDeclarationProvider>(sp => new DefaultDeclarationProvider("", DefaultNamespace));

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