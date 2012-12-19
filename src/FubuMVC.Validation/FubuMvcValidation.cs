using Bottles;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.UI;
using FubuMVC.Validation.Remote;
using FubuMVC.Validation.UI;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation
{
    public class FubuMvcValidation : IFubuRegistryExtension
    {
        void IFubuRegistryExtension.Configure(FubuRegistry registry)
        {
            registry.Services<FubuValidationServiceRegistry>();
            registry.Services<FubuMvcValidationServices>();
            registry.Actions.IncludeType<ValidateFieldEndpoint>();

            registry.Import<HtmlConventionRegistry>(x =>
            {
                x.Editors.Add(new FieldValidationElementModifier());
                x.Editors.Add(new RemoteValidationElementModifier());
            });

            registry.Policies.Add<ValidationConvention>();
        }
    }

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

    public class FubuValidationServiceRegistry : ServiceRegistry
    {
        public FubuValidationServiceRegistry()
        {
            SetServiceIfNone<ITypeResolver, TypeResolver>();
            SetServiceIfNone<IValidator, Validator>();

            setSingleton<ValidationGraph, ValidationGraph>();
            setSingleton<IFieldRulesRegistry, FieldRulesRegistry>();
            setSingleton<RemoteRuleGraph, RemoteRuleGraph>();
        }

        private void setSingleton<TPlugin, TConcrete>()
        {
            var def = SetServiceIfNone(typeof(TPlugin), typeof(TConcrete));
            def.IsSingleton = true;
        }
    }
}