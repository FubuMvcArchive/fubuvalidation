using FubuLocalization;
using FubuMVC.Validation.StoryTeller.Endpoints.User;
using FubuValidation;

namespace FubuMVC.Validation.StoryTeller.Endpoints.ClassValidation
{
    // SAMPLE: ClassValidationRules
    public class ClassValidationModelRules : ClassValidationRules<ClassValidationModel>
    {
        public ClassValidationModelRules()
        {
            Property(x => x.GreaterThanZero).GreaterThanZero();
            Property(x => x.GreaterOrEqualToZero).GreaterOrEqualToZero();
            Property(x => x.LongerThanTen).MinimumLength(10);
            Property(x => x.NoMoreThanFiveCharacters).MaximumLength(5);
            Property(x => x.AtLeastFiveButNotTen).RangeLength(5, 10);
            Property(x => x.GreaterThanFive).MinValue(5);
            Property(x => x.LessThanFifteen).MaxValue(15);
            Property(x => x.Email).Email();
            Property(x => x.Required).Required();
            Property(x => x.Regex).RegEx("[a-zA-Z0-9]+$");

            Property(x => x.Custom).Rule<UniqueUsernameRule>();

            Property(x => x.Password)
                .Matches(x => x.ConfirmPassword)
                .ReportErrorsOn(x => x.Password)
                .ReportErrorsOn(x => x.ConfirmPassword);

            Property(x => x.Email)
                .Matches(x => x.ConfirmEmail)
                .ReportErrorsOn(x => x.ConfirmEmail)
                .UseToken(StringToken.FromKeyString("Test:Keys", "Emails must match"));
        }
    }
    // ENDSAMPLE
}