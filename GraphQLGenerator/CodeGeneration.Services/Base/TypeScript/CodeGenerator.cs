using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base.Result;
using System.Text;

namespace CodeGeneration.Services.Base.TypeScript
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
    public abstract class CodeGenerator<TCodingUnit> : ICodeGenerator<TCodingUnit>
        where TCodingUnit : CodingUnit
    {
        private TCodingUnit? _codingUnit;

        protected virtual TCodingUnit CodingUnit => _codingUnit ?? throw new ApplicationException($"{CodingUnit} is not initiated.");
        public abstract GenerationResult Generate();

        public void Init(TCodingUnit codingUnit)
        {
            _codingUnit = codingUnit ?? throw new ArgumentNullException(nameof(codingUnit));
        }
    }
}
