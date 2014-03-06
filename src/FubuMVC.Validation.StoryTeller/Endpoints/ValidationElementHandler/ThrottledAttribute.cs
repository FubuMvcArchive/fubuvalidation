using System;
using System.Collections.Generic;
using System.Reflection;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation.StoryTeller.Endpoints.ValidationElementHandler
{
    public class ThrottledAttribute : FieldValidationAttribute
    {
        private readonly int _throttleSeconds;
        private readonly ValidationMode _mode;

        public ThrottledAttribute(int throttleSeconds) : this(throttleSeconds, ValidationMode.Live) { }

        public ThrottledAttribute(int throttleSeconds, string mode) : this(throttleSeconds, new ValidationMode(mode)) { }

        public ThrottledAttribute(int throttleSeconds, ValidationMode mode)
        {
            _throttleSeconds = throttleSeconds;
            _mode = mode;
        }

        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new ThrottledRule(TimeSpan.FromSeconds(_throttleSeconds), _mode);
        }
    }
}