using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Models.CodingUnits.Meta.Members;
using CodeGeneration.Services.Base;
using CodeGeneration.Services.Base.String;

namespace CodeGeneration.Services.Generators.TypeScriptGenerators
{
    public interface IModelGenerator : ICodeGenerator<Model>
    {
    }

    public class ModelGenerator : StringBasedClassGenerator<Model>, IModelGenerator
    {
        public ModelGenerator(
            ITypeScriptClassTemplate typeScriptClassTemplate,
            ITypeScriptPropertyGenerator typeScriptPropertyGenerator,
            ITypeScriptMethodGenerator typeScriptMethodGenerator) : base(typeScriptClassTemplate, typeScriptPropertyGenerator, typeScriptMethodGenerator)
        {
        }
    }

    public class TypeScriptPropertyGenerator : MemberCodeGenerator<PropertyInfo>, ITypeScriptPropertyGenerator
    {
        public TypeScriptPropertyGenerator(ITypeScriptPropertyTemplate stringBasedCodeTemplate) : base(stringBasedCodeTemplate)
        {
        }
    }
    public interface ITypeScriptPropertyGenerator: ICodeGenerator<PropertyInfo>
    {

    }

    public class TypeScriptMethodGenerator : MemberCodeGenerator<MethodInfo>, ITypeScriptMethodGenerator
    {
        public TypeScriptMethodGenerator(ITypeScriptMethodTemplate stringBasedCodeTemplate) : base(stringBasedCodeTemplate)
        {
        }
    }
    public interface ITypeScriptMethodGenerator : ICodeGenerator<MethodInfo>
    {

    }

    public class TypeScriptPropertyTemplate : ITypeScriptPropertyTemplate
    {
        public string Template => @"
    {{name}}:{{type}};";
    }

    public interface ITypeScriptPropertyTemplate : IStringBasedCodeTemplate
    {
    }

    public class TypeScriptMethodTemplate : ITypeScriptMethodTemplate
    {
        public string Template => @"
    function {{name}}(){
        return new {{type}}();
    }";
    }

    public interface ITypeScriptMethodTemplate : IStringBasedCodeTemplate
    {

    }

    public class TypeScriptClassTemplate : ITypeScriptClassTemplate
    {
        public string Template => @"
class {{name}} {

{{properties}}

{{methods}}
}
";
    }
    public interface ITypeScriptClassTemplate :IStringBasedCodeTemplate
    {

    }
}
