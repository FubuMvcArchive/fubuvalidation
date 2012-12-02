using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Validation
{
    public class ValidationActionFilter<T>
    {
        private readonly IValidationFilter<T> _filter;
        private readonly IFubuRequest _request;

        public ValidationActionFilter(IValidationFilter<T> filter, IFubuRequest request)
        {
            _filter = filter;
            _request = request;
        }

        public FubuContinuation Validate(T input)
        {
            var notification = _filter.Validate(input);
            if(notification.IsValid())
            {
                return FubuContinuation.NextBehavior();
            }

            _request.Set(notification);
            return FubuContinuation.TransferTo(input, categoryOrHttpMethod: "GET");
        }
    }
}