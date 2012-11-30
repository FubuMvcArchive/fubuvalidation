using FubuMVC.Core.Continuations;

namespace FubuMVC.Validation
{
    public class ValidationActionFilter<T>
    {
        private readonly IValidationFilter<T> _filter;

        public ValidationActionFilter(IValidationFilter<T> filter)
        {
            _filter = filter;
        }

        public FubuContinuation Validate(T input)
        {
            var notification = _filter.Validate(input);
            if(notification.IsValid())
            {
                return FubuContinuation.NextBehavior();
            }

            return FubuContinuation.TransferTo(input, categoryOrHttpMethod: "GET");
        }
    }
}