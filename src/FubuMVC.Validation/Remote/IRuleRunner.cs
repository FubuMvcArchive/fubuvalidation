using FubuCore;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation.Remote
{
    public interface IRuleRunner
    {
        Notification Run(RemoteFieldRule rule, string value);
    }

    public class RuleRunner : IRuleRunner
    {
        private readonly IServiceLocator _services;
        private readonly IValidator _validator;
        private readonly IValidationTargetResolver _resolver;

        public RuleRunner(IServiceLocator services, IValidator validator, IValidationTargetResolver resolver)
        {
            _services = services;
            _validator = validator;
            _resolver = resolver;
        }

        public Notification Run(RemoteFieldRule rule, string value)
        {
            var target = _resolver.Resolve(rule.Accessor, value);
            var fieldRule = _services.GetInstance(rule.Type).As<IFieldValidationRule>();


            var context = _validator.ContextFor(target, new Notification(target.GetType()));
            fieldRule.Validate(rule.Accessor, context);

            return context.Notification;
        }
    }
}