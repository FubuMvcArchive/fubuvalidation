using FubuMVC.Core.Continuations;

namespace FubuMVC.Validation
{
    public interface IFubuContinuationResolver
    {
        FubuContinuation Resolve(ValidationFailure context);
    }
}