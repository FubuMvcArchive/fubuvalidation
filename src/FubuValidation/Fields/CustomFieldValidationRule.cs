using System;
using FubuCore.Reflection;
using FubuLocalization;

namespace FubuValidation.Fields
{
    public class CustomFieldValidationRule<T> : IFieldValidationRule
    {
        private readonly Func<T, bool> theCondition;

        public CustomFieldValidationRule(Func<T, bool> condition)
        {
            theCondition = condition;
            Token = ValidationKeys.InvalidFormat;
        }

        public CustomFieldValidationRule(Func<T, bool> condition, StringToken token)
        {
            theCondition = condition;
            Token = token ?? ValidationKeys.InvalidFormat;
        }

        public void Validate(Accessor accessor, ValidationContext context)
        {
            if (theCondition((T)context.Target)) return;

            context.Notification.RegisterMessage(accessor, Token, new TemplateValue[0]);
        }

        public StringToken Token { get; set; }
        public ValidationMode Mode { get; set; }
    }
}