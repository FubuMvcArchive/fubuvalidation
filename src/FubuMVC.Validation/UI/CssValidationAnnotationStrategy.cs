using System;
using FubuCore.Util;
using FubuMVC.Core.UI.Elements;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
    public class CssValidationAnnotationStrategy : IValidationAnnotationStrategy
    {
        private static readonly Cache<Type, string> Classes = new Cache<Type, string>();

        static CssValidationAnnotationStrategy()
        {
            Classes[typeof (RequiredFieldRule)] = "required";
            Classes[typeof (GreaterThanZeroRule)] = "greater-than-zero";
            Classes[typeof (GreaterOrEqualToZeroRule)] = "greater-equal-zero";
        }

        public bool Matches(IFieldValidationRule rule)
        {
            return Classes.Has(rule.GetType());
        }

        public void Modify(ElementRequest request, IFieldValidationRule rule)
        {
            var tag = request.CurrentTag;
            tag.AddClass(Classes[rule.GetType()]);
        }
    }
}