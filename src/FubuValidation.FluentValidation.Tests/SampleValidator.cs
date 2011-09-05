using FluentValidation;

namespace FubuValidation.FluentValidation.Tests
{
    public class SampleValidator : AbstractValidator<SampleFluentModel>
    {
        public SampleValidator()
        {
            RuleFor(m => m.RequiredField)
                .NotEmpty();
        }
    }
}