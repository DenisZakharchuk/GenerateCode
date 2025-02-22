namespace CodeGeneration.Models.CodingUnits.Providers
{
    public interface ICodingUnitInfoProvider
    {
        string Name { get; }
        string? Namespace { get; }
        IEnumerable<string> RequiredNamespaces { get; }
        bool HasBase { get; }
    }
}
