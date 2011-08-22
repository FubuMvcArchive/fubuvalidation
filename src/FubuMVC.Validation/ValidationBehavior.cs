using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuValidation;

namespace FubuMVC.Validation
{
    public class ValidationBehavior<T> : BasicBehavior
        where T : class
    {
        private readonly IFubuRequest _request;
        private readonly ActionCall _target;
        private readonly IValidator _provider;
        private readonly IValidationFailureHandler _failureHandler;

        public ValidationBehavior(IFubuRequest request, ActionCall target, IValidator provider, IValidationFailureHandler failureHandler)
            : base(PartialBehavior.Executes)
        {
            _request = request;
            _target = target;
            _provider = provider;
            _failureHandler = failureHandler;
        }

        public ActionCall Target
        {
            get { return _target; }
        }

        protected override DoNext performInvoke()
        {
            var inputModel = _request.Get<T>();
            var notification = _provider.Validate(inputModel);
            if(notification.IsValid())
            {
                return DoNext.Continue;
            }

            _request.Set(notification);
            var context = new ValidationFailureContext(_target, notification, inputModel);
            _failureHandler.Handle(context);

            return DoNext.Stop;
        }
    }
}