using FubuCore.Reflection;

namespace FubuValidation.Fields
{
    public class MinimumLengthRule : IFieldValidationRule
    {
        public static readonly string LENGTH = "length";
        private readonly int _length;

        public MinimumLengthRule(int length)
        {
            _length = length;
        }

        public int Length
        {
            get { return _length; }
        }

        public void Validate(Accessor accessor, ValidationContext context)
        {
            var value = context.GetFieldValue<string>(accessor);
            if (value != null && value.Length < Length)
            {
                context.Notification.RegisterMessage(accessor, ValidationKeys.MinLength, TemplateValue.For(LENGTH, _length));
            }
        }

        public bool Equals(MinimumLengthRule other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._length == _length;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(MinimumLengthRule)) return false;
            return Equals((MinimumLengthRule)obj);
        }

        public override int GetHashCode()
        {
            return _length;
        }
    }
}