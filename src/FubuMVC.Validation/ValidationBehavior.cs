using System;
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
        private readonly IValidationFailureHandler _failureHandler;
        private readonly IModelBindingErrors _modelBindingErrors;
        private readonly IValidator _provider;
        private readonly IFubuRequest _request;
        private readonly ActionCall _target;

        public ValidationBehavior(IFubuRequest request, ActionCall target, IValidator provider, IValidationFailureHandler failureHandler, IModelBindingErrors modelBindingErrors)
            : base(PartialBehavior.Executes)
        {
            _request = request;
            _target = target;
            _provider = provider;
            _failureHandler = failureHandler;
            _modelBindingErrors = modelBindingErrors;
        }

        public ActionCall Target
        {
            get { return _target; }
        }

        protected override DoNext performInvoke()
        {
            var inputModel = _request.Get<T>();
            var notification = _provider.Validate(inputModel);


            _modelBindingErrors.AddAnyErrors<T>(notification);


            if (notification.IsValid())
            {
                return DoNext.Continue;
            }

            _request.Set(notification);
            var context = new ValidationFailure(_target, notification, inputModel);
            _failureHandler.Handle(context);

            return DoNext.Stop;
        }
    }
}