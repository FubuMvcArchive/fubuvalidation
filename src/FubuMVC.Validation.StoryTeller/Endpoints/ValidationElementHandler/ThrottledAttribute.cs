using System;
using System.Collections.Generic;
using System.Reflection;
using FubuValidation.Fields;

namespace FubuMVC.Validation.StoryTeller.Endpoints.ValidationElementHandler
{
    public class ThrottledAttribute : FieldValidationAttribute
    {
        private readonly int _throttleSeconds;

        public ThrottledAttribute(int throttleSeconds)
        {
            _throttleSeconds = throttleSeconds;
        }

        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new ThrottledRule(TimeSpan.FromSeconds(_throttleSeconds));
        }
    }
}