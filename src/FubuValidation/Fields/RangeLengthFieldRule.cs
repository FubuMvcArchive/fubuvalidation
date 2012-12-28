using System.Collections.Generic;
using FubuCore.Reflection;

namespace FubuValidation.Fields
{
    public class RangeLengthFieldRule : IFieldValidationRule
    {
        private readonly int _min;
        private readonly int _max;

        public RangeLengthFieldRule(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public void Validate(Accessor accessor, ValidationContext context)
        {
            var value = context.GetFieldValue<string>(accessor) ?? string.Empty;
            var length = value.Length;

            if(length < _min || length > _max)
            {
                var min = TemplateValue.For("Min", _min);
                var max = TemplateValue.For("Max", _max);

                context.Notification.RegisterMessage(accessor, ValidationKeys.RangeLength, min, max);
            }
        }

        public IDictionary<string, object> ToValues()
        {
            return new Dictionary<string, object>
                   {
                       { "min", _min }, { "max", _max }
                   };
        }
    }
}