namespace CodeGeneration.Models.CodingUnits.Meta.Members
{
    public abstract class BaseMember : CodingUnit
    {
        public required CodingUnit Type { get; set; }
    }
}
