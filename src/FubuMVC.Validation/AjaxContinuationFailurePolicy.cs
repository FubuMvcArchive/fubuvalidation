using FubuMVC.Core.Ajax;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Validation
{
    public class AjaxContinuationFailurePolicy : IValidationFailurePolicy
    {
        private readonly IAjaxContinuationResolver _continuationResolver;
        private readonly IJsonWriter _writer;

        public AjaxContinuationFailurePolicy(IAjaxContinuationResolver continuationResolver, IJsonWriter writer)
        {
            _continuationResolver = continuationResolver;
            _writer = writer;
        }

        public bool Matches(ValidationFailure context)
        {
            var type = context.Target.OutputType();
            if(type == null)
            {
                return false;
            }

            return typeof (AjaxContinuation).IsAssignableFrom(type);
        }

        public void Handle(ValidationFailure context)
        {
            var continuation = _continuationResolver.Resolve(context.Notification);
            _writer.Write(continuation.ToDictionary(), MimeType.Json.ToString());
        }
    }
}