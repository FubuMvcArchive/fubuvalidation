using System.Text.RegularExpressions;
using FubuCore.Reflection;

namespace FubuValidation.Fields
{
    public class EmailFieldRule : IFieldValidationRule
    {
        private static readonly Regex EmailExpression = new Regex(@"^(?:[\w\!\#\$\%\&\'\*\+\-\/\=\?\^\`\{\|\}\~]+\.)*[\w\!\#\$\%\&\'\*\+\-\/\=\?\^\`\{\|\}\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\-](?!\.)){0,61}[a-zA-Z0-9]?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\[(?:(?:[01]?\d{1,2}|2[0-4]\d|25[0-5])\.){3}(?:[01]?\d{1,2}|2[0-4]\d|25[0-5])\]))$", RegexOptions.Compiled);

        public void Validate(Accessor accessor, ValidationContext context)
        {
            var email = context.GetFieldValue<string>(accessor);
            if(!EmailExpression.IsMatch(email))
            {
                context.Notification.RegisterMessage(accessor, ValidationKeys.Email);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is EmailFieldRule;
        }
    }
}