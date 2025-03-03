namespace CodeGeneration.Services.Base.String
{
    public class TypeScriptPropertyCodeTemplate : StringBasedCodeTemplate, ITypeScriptPropertyCodeTemplate
    {
        public TypeScriptPropertyCodeTemplate(string template) : base(template)
        {
        }
    }
    public class TypeScriptMethodCodeTemplate : StringBasedCodeTemplate, ITypeScriptMethodCodeTemplate
    {
        public TypeScriptMethodCodeTemplate(string template) : base(template)
        {
        }
    }
    public class StringBasedCodeTemplate : IStringBasedCodeTemplate
    {
        private readonly string _template;

        public StringBasedCodeTemplate(string template)
        {
            _template = template;
        }

        public string Template => _template;
    }
    public interface ITypeScriptPropertyCodeTemplate : IStringBasedCodeTemplate
    { }

    public interface ITypeScriptMethodCodeTemplate : IStringBasedCodeTemplate
    { }
    public interface IStringBasedCodeTemplate
    {
        string Template { get; }
    }
}
