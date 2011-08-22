using System;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Validation
{
    public class FubuContinuationFailurePolicy : IValidationFailurePolicy
    {
        private readonly Func<ValidationFailureContext, bool> _predicate;
        private readonly IFubuRequest _request;
        private readonly IFubuContinuationResolver _continuationResolver;
        private readonly ContinuationHandler _handler;

        public FubuContinuationFailurePolicy(Func<ValidationFailureContext, bool> predicate, IFubuRequest request,
                                             IFubuContinuationResolver continuationResolver, ContinuationHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
            _continuationResolver = continuationResolver;
            _request = request;
        }

        public bool Matches(ValidationFailureContext context)
        {
            return _predicate(context);
        }

        public void Handle(ValidationFailureContext context)
        {
            var continuation = _continuationResolver.Resolve(context);
            _request.Set(continuation);
            _handler.Invoke();
        }
    }
}