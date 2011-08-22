using System;
using System.Collections.Generic;
using FubuMVC.Core.Registration.ObjectGraph;

namespace FubuMVC.Validation.Registration
{
    public class ValidationFailureExpression
    {
        private readonly IList<ObjectDef> _policies;

        public ValidationFailureExpression(IList<ObjectDef> policies)
        {
            _policies = policies;
        }

        public ValidationFailureExpression ApplyPolicy<TPolicy>()
            where TPolicy : IValidationFailurePolicy, new()
        {
            return ApplyPolicy(new TPolicy());
        }

        public ValidationFailureExpression ApplyPolicy(IValidationFailurePolicy policy)
        {
            _policies.Add(new ObjectDef
                              {
                                  Type = typeof(IValidationFailurePolicy),
                                  Value = policy
                              });
            return this;
        }

        public ConfigureModelValidationFailureExpression If(Func<ValidationFailureContext, bool> predicate)
        {
            return new ConfigureModelValidationFailureExpression(predicate, _policies);
        }

        public ConfigureModelValidationFailureExpression IfModelIs<T>()
            where T : class
        {
            return If(context => context.InputType().Equals(typeof (T)));
        }
    }
}