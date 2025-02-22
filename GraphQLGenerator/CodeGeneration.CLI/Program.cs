using CodeGeneration.Models.CodingUnits.Meta;

internal class Program
{
    private static void Main(string[] args)
    {
        var classInfo = new CodingUnit() { 
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

        
    }
}