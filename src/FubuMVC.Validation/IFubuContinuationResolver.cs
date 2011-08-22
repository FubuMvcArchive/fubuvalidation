using FubuMVC.Core.Continuations;

namespace FubuMVC.Validation
{
    public interface IFubuContinuationResolver
    {
        FubuContinuation Resolve(ValidationFailureContext context);
    }
}