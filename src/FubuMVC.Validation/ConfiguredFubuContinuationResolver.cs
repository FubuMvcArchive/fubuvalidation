using FubuMVC.Core.Continuations;

namespace FubuMVC.Validation
{
    public class ConfiguredFubuContinuationResolver : IFubuContinuationResolver
    {
        private readonly FubuContinuation _continuation;

        public ConfiguredFubuContinuationResolver(FubuContinuation continuation)
        {
            _continuation = continuation;
        }

        public FubuContinuation Resolve(ValidationFailure context)
        {
            return _continuation;
        }
    }
}