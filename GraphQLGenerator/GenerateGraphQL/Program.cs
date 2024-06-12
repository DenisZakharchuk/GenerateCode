using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GQLG.CodeGeneration.Base;
using GQLG.CodeGeneration.GraphQL;
using GQLG.Models.Factories;
using GQLG.Models.Meta;
using Microsoft.CodeAnalysis;

namespace GQLG
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check command line arguments
            if (args.Length != 4 || args[0] != "-in" || args[2] != "-out")
            {
                Console.WriteLine("Usage: GenerateGraphQL -in <input_folder> -out <output_folder>");
                return;
            }

            string inputFolder = args[1];
            string outputFolder = args[3];

            // Ensure output directory exists
            Directory.CreateDirectory(outputFolder);

            // Load the assembly from the input folder
            var dllFiles = Directory.GetFiles(inputFolder, "*.dll");
            foreach (var dllFile in dllFiles)
            {
                Assembly assembly = Assembly.LoadFrom(dllFile);

                var types = assembly.GetTypes().Where(t => t.IsClass && t.GetProperties().Any());

                var summaryClass = new ClassInfo() { Name = "Integration" };
                var properties = new List<Models.Meta.PropertyInfo>();

                foreach (var type in types)
                {
                    var graphQLDataReaderGenerator = new GraphQLDataReaderGenerator(c => $"MijDim.Web.GraphQL.{c.Name}.Types.DataReader");

                    var classInfo = ClassInfoFactory.Build(type);

                    properties.Add(new Models.Meta.PropertyInfo()
                    {
                        Name = classInfo.Name,
                        Type = $"{graphQLDataReaderGenerator.Namespace(classInfo)}.{graphQLDataReaderGenerator.GetClassName(classInfo.Name)}"
                    });

                    var codeGenerators = new CodeGenerator[] { 
                        new GraphQLTypeGenerator(c => $"MijDim.Web.GraphQL.{c.Name}.Types.Object"),
                        new GraphQLFilterTypeGenerator(c => $"MijDim.Web.GraphQL.{c.Name}.Types.Filters"),
                        new GraphQLOrderTypeGenerator(c => $"MijDim.Web.GraphQL.{c.Name}.Types.Orders"),
                        graphQLDataReaderGenerator,
                        new GraphQLDataReaderInterfaceGenerator(c => $"MijDim.Web.GraphQL.{c.Name}.Types.DataReader"),
                    };

                    foreach (var codeGenerator in codeGenerators)
                    {
                        // Generate GraphQL type class
                        var syntaxTree = codeGenerator.Generate(classInfo);

                        // Save generated class file
                        var outputSubFolder = Path.Combine(outputFolder, type.Name, codeGenerator.SubDir());

                        // Ensure output directory exists
                        Directory.CreateDirectory(outputSubFolder);
                        
                        var outputCsPath = Path.Combine(outputSubFolder, $"{type.Name}{codeGenerator.CodeKind()}.cs");

                        File.WriteAllText(outputCsPath, syntaxTree.ToString());
                    }
                }

                summaryClass.Properties = properties.ToArray();
                var queryCodeGenerator = new GraphQLQueryGenerator(c => $"MijDim.Web.GraphQL.{c.Name}.Query");

                var queryClass = queryCodeGenerator.Generate(summaryClass);
                var queryOutputSubFolder = Path.Combine(outputFolder, queryCodeGenerator.SubDir());
                Directory.CreateDirectory(queryOutputSubFolder);
                var outputQueryPath = Path.Combine(queryOutputSubFolder, $"{summaryClass.Name}{queryCodeGenerator.CodeKind()}.cs");
                File.WriteAllText(outputQueryPath, queryClass.ToString());

            }
        }
    }
}