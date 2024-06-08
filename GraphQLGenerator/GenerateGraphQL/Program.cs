using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
                Console.WriteLine("Usage: GQLG -in <input_folder> -out <output_folder>");
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
                var baseEntityType = typeof(BaseEntity);

                // Find all subclasses of BaseEntity
                var types = assembly.GetTypes().Where(t => t.IsClass && t.GetProperties().Any());

                foreach (var type in types)
                {
                    // Generate JSON string for the type
                    string json = ClassInfoGenerator.Generate(type);

                    // Save JSON file
                    string outputJsonPath = Path.Combine(outputFolder, type.Name + ".json");
                    //File.WriteAllText(outputJsonPath, json);

                    // Deserialize JSON to PropertyInfo array
                    var properties = Newtonsoft.Json.JsonConvert.DeserializeObject<PropertyInfo[]>(json);

                    // Generate GraphQL type class
                    SyntaxTree syntaxTree = GraphQLTypeGenerator.Generate(properties, type.Name);
                    
                    // Save generated class file
                    string outputCsPath = Path.Combine(outputFolder, type.Name + "GraphQLType.cs");
                    File.WriteAllText(outputCsPath, syntaxTree.ToString());
                }
            }
        }

        // Generate method to create JSON string of public properties
        public static string Generate(Type target)
        {
            var properties = target.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                   .Select(p => new { p.Name, Type = p.PropertyType.Name })
                                   .ToList();

            return JsonConvert.SerializeObject(properties, Formatting.Indented);
        }
    }

    // BaseEntity class definition for demonstration purposes
    public abstract class BaseEntity
    {
    }

    // Example derived class for demonstration purposes
    public class ExampleEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}