namespace FubuMVC.Validation
{
    public class FubuContinuationModelResolver : IFubuContinuationModelResolver
    {
        private readonly IFubuContinuationModelDescriptor _descriptor;
        private readonly IInputModelResolver _modelResolver;

        public FubuContinuationModelResolver(IFubuContinuationModelDescriptor descriptor, IInputModelResolver modelResolver)
        {
            _descriptor = descriptor;
            _modelResolver = modelResolver;
        }

        public object ModelFor(ValidationFailureContext context)
        {
            var destinationType = _descriptor.DescribeModelFor(context);
            return _modelResolver.Resolve(destinationType, context.InputType(), context.InputModel);
        }
    }
}