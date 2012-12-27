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
            registry.Actions.FindWith<RemoteRulesSource>();
            registry.Actions.FindWith<ValidationSummarySource>();

            registry.Import<HtmlConventionRegistry>(x =>
            {
                x.Editors.Add(new FieldValidationElementModifier());
                x.Editors.Add(new RemoteValidationElementModifier());
                
                x.Forms.Add(new FormValidationSummaryModifier());
                x.Forms.Add(new FormActivationModifier());
            });

            registry.Policies.Add<ValidationConvention>();
            registry.Policies.Add<AttachDefaultValidationSummary>();
            registry.Policies.Add<RegisterRemoteRuleQuery>();

            registry.AlterSettings<ValidationSettings>(settings =>
            {
                settings
                    .Remotes
                    .FindWith<RemoteRuleAttributeFilter>()
                    .FindWith<RemoteFieldValidationRuleFilter>();
            });
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