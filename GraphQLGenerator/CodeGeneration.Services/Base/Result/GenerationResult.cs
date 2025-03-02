namespace CodeGeneration.Services.Base.Result
{
    public class GenerationResult
    {

    }
    public class GenerationResult<TInner> : GenerationResult
        where TInner : class
    {
        readonly TInner inner;

        public GenerationResult(TInner inner)
        {
            this.inner = inner;
        }

        public override string ToString()
        {
            return inner.ToString() ?? string.Empty;
        }
    }
}
