using CodeGeneration.Models.CodingUnits.Meta.Members;

namespace CodeGeneration.Services.Base.String
{
    public class TypeScriptPropertyGenerator : MemberCodeGenerator<PropertyInfo>, ITypeScriptPropertyGenerator
    {
        public TypeScriptPropertyGenerator(ITypeScriptPropertyCodeTemplate stringBasedCodeTemplate) : base(stringBasedCodeTemplate)
        {
        }
    }
    public interface ITypeScriptPropertyGenerator : ICodeGenerator<PropertyInfo>
    {

    }
}
