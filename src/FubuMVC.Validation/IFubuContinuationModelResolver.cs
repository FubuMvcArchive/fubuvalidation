using System;

namespace FubuMVC.Validation
{
    public interface IFubuContinuationModelResolver
    {
        object ModelFor(ValidationFailure context);
    }

    public class LambdaFubuContinuationModelResolver : IFubuContinuationModelResolver
    {
        private readonly Func<ValidationFailure, object> _converter;

        public LambdaFubuContinuationModelResolver(Func<ValidationFailure, object> converter)
        {
            _converter = converter;
        }

        public object ModelFor(ValidationFailure context)
        {
            return _converter(context);
        }
    }
}