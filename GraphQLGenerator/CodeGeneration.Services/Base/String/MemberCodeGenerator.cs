using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base.Result;
using System.Text;

namespace CodeGeneration.Services.Base.String
{
    public abstract class MemberCodeGenerator<TCodingUnit> : CodeGenerator<TCodingUnit>
        where TCodingUnit : BaseMember
    {
        private readonly IStringBasedCodeTemplate _stringBasedCodeTemplate;

        protected MemberCodeGenerator(IStringBasedCodeTemplate stringBasedCodeTemplate)
        {
            _stringBasedCodeTemplate = stringBasedCodeTemplate;
        }

        public override GenerationResult Generate()
        {
            var builder = new StringBuilder(_stringBasedCodeTemplate.Template);
            builder.Replace("{{name}}", CodingUnit.Name);
            builder.Replace("{{type}}", CodingUnit.Type.Name);

            return new GenerationResult<StringBuilder>(builder);
        }
    }
}
