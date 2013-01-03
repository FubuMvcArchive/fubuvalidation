using FubuMVC.Core.Registration.Policies;

namespace FubuMVC.Validation
{
    public class DefaultValidationChainFilter : CompositeChainFilter
    {
        public DefaultValidationChainFilter()
            : base(new HasInputType(), new HttpMethodFilter("POST"), new NotValidatedAttributeFilter())
        {
        }
    }
}