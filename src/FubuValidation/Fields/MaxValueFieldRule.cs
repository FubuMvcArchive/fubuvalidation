using System;
using FubuCore.Reflection;

namespace FubuValidation.Fields
{
    public class MaxValueFieldRule : IFieldValidationRule
    {
        private readonly IComparable _bounds;

        public MaxValueFieldRule(IComparable bounds)
        {
            _bounds = bounds;
        }

        public IComparable Bounds { get { return _bounds; }}

        public void Validate(Accessor accessor, ValidationContext context)
        {
            var value = accessor.GetValue(context.Target);
            if(_bounds.CompareTo(value) < 0)
            {
                context.Notification.RegisterMessage(accessor, ValidationKeys.MaxValue, TemplateValue.For("Bounds", _bounds));
            }
        }
    }
}