using System.Collections.Generic;
using System.Reflection;
using FubuValidation.Fields;

namespace FubuValidation
{
    public class RangeLengthAttribute : FieldValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public RangeLengthAttribute(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public int Min
        {
            get { return _min; }
        }

        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new RangeLengthFieldRule(_min, _max);
        }
    }
}