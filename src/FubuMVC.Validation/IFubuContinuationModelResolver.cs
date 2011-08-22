using System;

namespace FubuMVC.Validation
{
    public interface IFubuContinuationModelResolver
    {
        object ModelFor(ValidationFailureContext context);
    }

    public class LambdaFubuContinuationModelResolver : IFubuContinuationModelResolver
    {
        private readonly Func<ValidationFailureContext, object> _converter;

        public LambdaFubuContinuationModelResolver(Func<ValidationFailureContext, object> converter)
        {
            _converter = converter;
        }

        public object ModelFor(ValidationFailureContext context)
        {
            return _converter(context);
        }
    }
}