using FubuCore;
using FubuLocalization;
using FubuValidation.Fields;

namespace FubuValidation
{
    public class ValidationKeys : StringToken
    {
        public static readonly ValidationKeys InvalidFormat = new ValidationKeys("Data is formatted incorrectly");
        public static readonly StringToken Required = new ValidationKeys("Required Field");
        public static readonly StringToken CollectionLength = new ValidationKeys("Must be exactly {0} element(s)".ToFormat(CollectionLengthRule.LENGTH.AsTemplateField()));
        public static readonly StringToken MaxLength = new ValidationKeys("Maximum length exceeded. Must be less than or equal to {0}".ToFormat(MaximumLengthRule.LENGTH.AsTemplateField()));
        public static readonly StringToken MinLength = new ValidationKeys("Minimum length not met. Must be greater than or equal to {0}".ToFormat(MinimumLengthRule.LENGTH.AsTemplateField()));
        public static readonly StringToken GreaterThanZero = new ValidationKeys("Value must be greater than zero");
        public static readonly StringToken GreaterThanOrEqualToZero = new ValidationKeys("Value must be greater than or equal to zero");

        public ValidationKeys(string text)
            : base(text, text, namespaceByType: true)
        {   
        }
    }
}