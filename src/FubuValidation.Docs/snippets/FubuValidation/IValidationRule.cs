namespace FubuValidation
{
	// SAMPLE: IValidationRule
    public interface IValidationRule
    {
        void Validate(ValidationContext context);
    }
	// ENDSAMPLE
}