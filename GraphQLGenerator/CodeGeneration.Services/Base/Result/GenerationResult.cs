namespace CodeGeneration.Services.Base.Result
{
    public class GenerationResult
    {
        public static GenerationResult<TResult> From<TResult>(TResult result)
            where TResult : class
        {
            return new GenerationResult<TResult>(result);
        }
    }
    public class GenerationResult<TInner> : GenerationResult
        where TInner : class
    {
        readonly TInner inner;

        public GenerationResult(TInner inner)
        {
            this.inner = inner;
        }

        public TInner Result => inner;

        public override string ToString()
        {
            return inner.ToString() ?? string.Empty;
        }
    }
}
