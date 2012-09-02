using System.Collections.Generic;
using FubuMVC.Core;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Validation.Registration;

namespace FubuMVC.Validation
{
    public class FubuValidation : IFubuRegistryExtension
    {
        private readonly ValidationCallMatcher _callMatcher = new ValidationCallMatcher();
        private readonly IList<ObjectDef> _validationPolicies = new List<ObjectDef>();

        public FubuValidation()
        {
            setDefaults();
        }

        public ValidationCandidateExpression Actions
        {
            get { return new ValidationCandidateExpression(_callMatcher); }
        }

        public ValidationFailureExpression Failures
        {
            get { return new ValidationFailureExpression(_validationPolicies); }
        }

        void IFubuRegistryExtension.Configure(FubuRegistry registry)
        {
            registry.Services(x =>
            {
                x.SetServiceIfNone<IValidationContinuationHandler, ValidationContinuationHandler>();
                x.SetServiceIfNone<IAjaxContinuationResolver, AjaxContinuationResolver>();
                x.SetServiceIfNone<IAjaxContinuationActivator, AjaxContinuationActivator>();
                x.AddService<IAjaxContinuationDecorator, StandardAjaxContinuationDecorator>();

                x.SetServiceIfNone<IModelBindingErrors, ModelBindingErrors>();

                x.SetServiceIfNone<IValidationFailureHandler, ValidationFailureHandler>();
                _validationPolicies
                    .Each(policy => x.AddService(typeof (IValidationFailurePolicy), policy));
            });

            registry
                .Policies
                .Add(new ReorderBehaviorsPolicy
                         {
                             WhatMustBeBefore = node => node is ValidationNode,
                             WhatMustBeAfter = node => node is OutputNode
                         });

            registry
                .ApplyConvention(new ValidationConvention(_callMatcher.CallFilters.Matches));
        }

        private void setDefaults()
        {
            Failures
                .ApplyPolicy<AjaxContinuationFailurePolicy>();
        }
    }
}