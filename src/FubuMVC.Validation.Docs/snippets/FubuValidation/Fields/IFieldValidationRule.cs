using FubuCore.Reflection;
using FubuLocalization;

namespace FubuValidation.Fields
{
	// SAMPLE: IFieldValidationRule
	public interface IFieldValidationRule
	{
		StringToken Token { get; set; }

		void Validate(Accessor accessor, ValidationContext context);
	}
	// ENDSAMPLE
}