using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Providers;

internal class Program
{
    private static void Main(string[] args)
    {
        var modelInfo = new CodingUnit() { 
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
        };

        var providers = new ICodingUnitInfoProvider[]
        {
            new DataModelInfoProvider(modelInfo),
            new ClassInfoProvider(modelInfo),
        }
    }
}