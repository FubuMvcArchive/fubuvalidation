using System.Net;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Validation
{
    public class AjaxValidationActionFilter<T>
    {
        private readonly IValidationFilter<T> _filter;
        private readonly IAjaxContinuationResolver _resolver;
        private readonly IJsonWriter _writer;

        public AjaxValidationActionFilter(IValidationFilter<T> filter, IAjaxContinuationResolver resolver, IJsonWriter writer)
        {
            _filter = filter;
            _resolver = resolver;
            _writer = writer;
        }

        public FubuContinuation Validate(T input)
        {
            var notification = _filter.Validate(input);
            if(notification.IsValid())
            {
                return FubuContinuation.NextBehavior();
            }

            var continuation = _resolver.Resolve(notification);
            _writer.Write(continuation.ToDictionary(), MimeType.Json.Value);

            return FubuContinuation.EndWithStatusCode(HttpStatusCode.InternalServerError);
        }
    }
}