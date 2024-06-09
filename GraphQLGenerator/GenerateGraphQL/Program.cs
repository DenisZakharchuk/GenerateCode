using System;
using System.IO;
using System.Linq;
using System.Reflection;
using GQLG.CodeGeneration.Base;
using GQLG.Models.Factories;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

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
                    var json = ClassInfoFactory.Create(type);

                    // Save JSON file
                    var outputJsonPath = Path.Combine(outputFolder, type.Name + ".json");
                    //File.WriteAllText(outputJsonPath, json);

                    // Deserialize JSON to PropertyInfo array
                    var properties = JsonConvert.DeserializeObject<Models.Meta.PropertyInfo[]>(json);

                    var graphQLTypeGenerators = new CodeGenerator[] { 
                        new GraphQLTypeGenerator() 
                    };

                    foreach (var graphQLTypeGenerator in graphQLTypeGenerators)
                    {
                        // Generate GraphQL type class
                        SyntaxTree syntaxTree = graphQLTypeGenerator.Generate(properties, type.Name);

                        // Save generated class file
                        var outputCsPath = Path.Combine(outputFolder, type.Name + "GraphQLType.cs");
                        File.WriteAllText(outputCsPath, syntaxTree.ToString());
                    }                    
                }
            }
        }
    }
}