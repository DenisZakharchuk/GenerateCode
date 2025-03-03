using CodeGeneration.Models.CodingUnits.Meta.Members;

namespace CodeGeneration.Services.Base.String
{
    public class TypeScriptMethodGenerator : MemberCodeGenerator<MethodInfo>, ITypeScriptMethodGenerator
    {
        public TypeScriptMethodGenerator(ITypeScriptMethodCodeTemplate stringBasedCodeTemplate) : base(stringBasedCodeTemplate)
        {
        }
    }
    public interface ITypeScriptMethodGenerator : ICodeGenerator<MethodInfo>
    {

    }
}
