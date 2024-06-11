using System;
using System.IO;
using System.Linq;
using System.Reflection;
using GQLG.CodeGeneration.Base;
using GQLG.CodeGeneration.GraphQL;
using GQLG.Models.Factories;
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

                foreach (var type in types)
                {
                    // Generate JSON string for the type
                    var classInfo = ClassInfoFactory.Build(type);

                    var codeGenerators = new CodeGenerator[] { 
                        new GraphQLTypeGenerator(c => $"MijDim.Web.GraphQL.{c.Name}.Types.Object"),
                        new GraphQLFilterTypeGenerator(c => $"MijDim.Web.GraphQL.{c.Name}.Types.Filters"),
                        new GraphQLOrderTypeGenerator(c => $"MijDim.Web.GraphQL.{c.Name}.Types.Orders"),
                        new GraphQLDataReaderGenerator(c => $"MijDim.Web.GraphQL.{c.Name}.Types.DataReader"),
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
            }
        }
    }
}