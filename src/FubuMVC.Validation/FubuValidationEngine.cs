using System.Collections.Generic;
using FubuCore.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Validation.Registration;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation
{
    public class FubuValidationEngine : IFubuRegistryExtension
    {
        private readonly ValidationRegistry _validationRegistry;
        private readonly IList<ObjectDef> _validationPolicies = new List<ObjectDef>();
        private readonly ValidationCallMatcher _callMatcher = new ValidationCallMatcher();

        public FubuValidationEngine()
            : this(new ValidationRegistry())
        {   
        }

        public FubuValidationEngine(ValidationRegistry validationRegistry)
        {
            _validationRegistry = validationRegistry;
        }

        public ValidationCandidateExpression Actions { get { return new ValidationCandidateExpression(_callMatcher); }}
        public ValidationFailureExpression Failures { get { return new ValidationFailureExpression(_validationPolicies); }}

        void IFubuRegistryExtension.Configure(FubuRegistry registry)
        {
            registry
                .Services(x =>
                              {
                                  var validationRegistry = _validationRegistry as IValidationRegistration;
                                  var rulesRegistry = new FieldRulesRegistry(validationRegistry.FieldSources(), new TypeDescriptorCache());
                                  validationRegistry.RegisterFieldRules(rulesRegistry);

                                  x.AddService<IValidationSource>(new FieldRuleSource(rulesRegistry));
                                  x.SetServiceIfNone<IFieldRulesRegistry>(rulesRegistry);
                                  x.SetServiceIfNone<IValidator, Validator>();
                                  x.SetServiceIfNone<IValidationContinuationHandler, ValidationContinuationHandler>();

                                  x.SetServiceIfNone<IValidationFailureHandler, ValidationFailureHandler>();
                                  _validationPolicies
                                      .Each(policy => x.AddService(typeof(IValidationFailurePolicy), policy));
                              });

            registry
                .ApplyConvention(new ValidationConvention(_callMatcher.CallFilters.Matches));
        }
    }
}