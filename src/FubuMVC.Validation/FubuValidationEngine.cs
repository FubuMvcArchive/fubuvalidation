using System.Collections.Generic;
using FubuMVC.Core;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Validation.Registration;

namespace FubuMVC.Validation
{
    public class FubuValidationEngine : IFubuRegistryExtension
    {
        private readonly IList<ObjectDef> _validationPolicies = new List<ObjectDef>();
        private readonly ValidationCallMatcher _callMatcher = new ValidationCallMatcher();

        public FubuValidationEngine()
        {
            setDefaults();
        }

        private void setDefaults()
        {
            Failures
                .ApplyPolicy<AjaxContinuationFailurePolicy>();
        }

        public ValidationCandidateExpression Actions { get { return new ValidationCandidateExpression(_callMatcher); }}
        public ValidationFailureExpression Failures { get { return new ValidationFailureExpression(_validationPolicies); }}

        void IFubuRegistryExtension.Configure(FubuRegistry registry)
        {
            registry
                .Services(x =>
                              {
                                  x.SetServiceIfNone<IValidationContinuationHandler, ValidationContinuationHandler>();
                                  x.SetServiceIfNone<IAjaxContinuationResolver, AjaxContinuationResolver>();
                                  x.SetServiceIfNone<IAjaxContinuationActivator, AjaxContinuationActivator>();
                                  x.AddService<IAjaxContinuationDecorator, StandardAjaxContinuationDecorator>();

                                  x.SetServiceIfNone<IValidationFailureHandler, ValidationFailureHandler>();
                                  _validationPolicies
                                      .Each(policy => x.AddService(typeof(IValidationFailurePolicy), policy));
                              });

            registry
                .ApplyConvention(new ValidationConvention(_callMatcher.CallFilters.Matches));
        }
    }
}