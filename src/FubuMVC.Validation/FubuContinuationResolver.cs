using System;
using FubuMVC.Core.Continuations;

namespace FubuMVC.Validation
{
    public class FubuContinuationResolver : IFubuContinuationResolver
    {
        private readonly IFubuContinuationModelResolver _resolver;
        private readonly Func<object, FubuContinuation> _continuationBuilder;

        public FubuContinuationResolver(IFubuContinuationModelResolver resolver, Func<object, FubuContinuation> continuationBuilder)
        {
            _resolver = resolver;
            _continuationBuilder = continuationBuilder;
        }

        public FubuContinuation Resolve(ValidationFailure context)
        {
            return _continuationBuilder(_resolver.ModelFor(context));
        }
    }
}