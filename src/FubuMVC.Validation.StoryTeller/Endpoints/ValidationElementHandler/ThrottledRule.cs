using System;
using System.Threading;
using FubuCore.Reflection;
using FubuLocalization;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation.StoryTeller.Endpoints.ValidationElementHandler
{
    public class ThrottledRule : IFieldValidationRule
    {
        private readonly TimeSpan _throttle;

        public ThrottledRule(TimeSpan throttle, ValidationMode mode)
        {
            _throttle = throttle;
            Mode = mode;
        }

        public StringToken Token { get; set; }
        public ValidationMode Mode { get; set; }

        public void Validate(Accessor accessor, ValidationContext context)
        {
            Thread.Sleep(_throttle);
        }
    }
}