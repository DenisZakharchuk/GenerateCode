namespace CodeGeneration.Models.CodingUnits.Providers.Default
{
    public class DefaultBaseClassInfoProvider : IDefaultBaseClassInfoProvider
    {
        public string Name => "Object";

        public string Namespace => "System";

        public IEnumerable<string> RequiredNamespaces { get { yield return "System"; } }

        public bool HasBase => false;
    }
    public interface IDefaultBaseClassInfoProvider : IModelInfoProvider
    {
    }
}
