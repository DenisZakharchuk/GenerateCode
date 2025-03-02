using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base.Result;
using System.Text;

namespace CodeGeneration.Services.Base.TypeScript
{
    public abstract class SingleClassGenerator<TCodingUnit> : CodeGenerator<TCodingUnit>
        where TCodingUnit: Class
    {
        private const string propertiesKey = "{{properties}}";
        private const string methodsKey = "{{methods}}";
        private const string constructorsKeys = "{{constructors}}";

        private readonly IStringBasedCodeTemplate _typeScriptClassTemplate;
        private readonly ICodeGenerator<PropertyInfo> _typeScriptPropertyGenerator;
        private readonly ICodeGenerator<MethodInfo> _typeScriptMethodGenerator;

        protected SingleClassGenerator(
            IStringBasedCodeTemplate typeScriptClassTemplate,
            ICodeGenerator<PropertyInfo> typeScriptPropertyGenerator,
            ICodeGenerator<MethodInfo> typeScriptMethodGenerator)
        {
            _typeScriptClassTemplate = typeScriptClassTemplate;
            _typeScriptPropertyGenerator = typeScriptPropertyGenerator;
            _typeScriptMethodGenerator = typeScriptMethodGenerator;
        }

        public override GenerationResult Generate()
        {
            var builder = new StringBuilder(_typeScriptClassTemplate.Template);
            var propertiesCode = new StringBuilder();
            
            if (CodingUnit.Properties != null)
            {
                foreach (var prop in CodingUnit.Properties)
                {
                    _typeScriptPropertyGenerator.Init(prop);
                    propertiesCode.Append(_typeScriptPropertyGenerator.Generate());
                }
            }            
            builder.Replace(SingleClassGenerator<TCodingUnit>.propertiesKey, propertiesCode.ToString());

            var methodsCode = new StringBuilder();

            if (CodingUnit.Methods != null)
            {
                foreach (var methodInfo in CodingUnit.Methods)
                {
                    _typeScriptMethodGenerator.Init(methodInfo);
                    methodsCode.Append(_typeScriptMethodGenerator.Generate());
                }
            }
            builder.Replace(SingleClassGenerator<TCodingUnit>.methodsKey, methodsCode.ToString());


            return new GenerationResult<StringBuilder>(builder);
        }
    }
}
