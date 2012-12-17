using System;
using System.Collections.Generic;
using FubuCore;

namespace FubuValidation
{
    public interface IValidated
    {
        void Validate(ValidationContext context);
    }

    public class SelfValidatingClassRule : IValidationRule
    {
        public void Validate(ValidationContext context)
        {
            context.Target.As<IValidated>().Validate(context);
        }
    }

    public class SelfValidatingClassRuleSource : IValidationSource
    {
        public IEnumerable<IValidationRule> RulesFor(Type type)
        {
            if (type.CanBeCastTo<IValidated>())
            {
                yield return new SelfValidatingClassRule();
            }
        }

        public override bool Equals(object obj)
        {
            return obj is SelfValidatingClassRuleSource;
        }
    }
}