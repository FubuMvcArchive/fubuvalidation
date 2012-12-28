using Bottles;
using FubuMVC.Core.Registration;
using FubuMVC.Validation.Remote;
using FubuMVC.Validation.UI;

namespace FubuMVC.Validation
{
    public class FubuMvcValidationServices : ServiceRegistry
    {
        public FubuMvcValidationServices()
        {
            SetServiceIfNone<IAjaxContinuationResolver, AjaxContinuationResolver>();
            SetServiceIfNone<IModelBindingErrors, ModelBindingErrors>();
            SetServiceIfNone<IAjaxValidationFailureHandler, AjaxValidationFailureHandler>();
            SetServiceIfNone<IValidationTargetResolver, ValidationTargetResolver>();
            SetServiceIfNone<IRuleRunner, RuleRunner>();
            SetServiceIfNone(typeof(IValidationFilter<>), typeof(ValidationFilter<>));
            SetServiceIfNone<IFieldValidationModifier, FieldValidationModifier>();

            AddService<IActivator, RemoteRuleGraphActivator>();
            AddService<IValidationAnnotationStrategy, CssValidationAnnotationStrategy>();
        }
    }
}