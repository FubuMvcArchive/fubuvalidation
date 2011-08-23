using System;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Validation
{
    public class FubuContinuationFailurePolicy : IValidationFailurePolicy
    {
        private readonly Func<ValidationFailure, bool> _predicate;
        private readonly IFubuRequest _request;
        private readonly IFubuContinuationResolver _continuationResolver;
        private readonly IValidationContinuationHandler _handler;

        public FubuContinuationFailurePolicy(Func<ValidationFailure, bool> predicate, IFubuRequest request,
                                             IFubuContinuationResolver continuationResolver, IValidationContinuationHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
            _continuationResolver = continuationResolver;
            _request = request;
        }

        public bool Matches(ValidationFailure context)
        {
            return _predicate(context);
        }

        public void Handle(ValidationFailure context)
        {
            var continuation = _continuationResolver.Resolve(context);
            _request.Set(continuation);
            _handler.Handle();
        }
    }
}