using FubuCore.Reflection;
using FubuLocalization;

namespace FubuValidation.Fields
{
    public class ContinuationFieldRule : IFieldValidationRule
    {
	    public StringToken Token { get; set; }

		public ValidationMode Mode { get; set; }

	    public void Validate(Accessor accessor, ValidationContext context)
        {
            context.ContinueValidation(accessor);
        }
    }
}