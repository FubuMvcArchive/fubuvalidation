using FubuMVC.Core.Registration;
using FubuMVC.Validation.StoryTeller.Endpoints.User;
using FubuValidation;

namespace FubuMVC.Validation.StoryTeller.Endpoints.AccessorOverrides
{
    public class AccessorOverridesModelRules : OverridesFor<AccessorOverridesModel>
    {
        public AccessorOverridesModelRules()
        {
            Property(x => x.GreaterThanZero).GreaterThanZero();
            Property(x => x.GreaterOrEqualToZero).GreaterOrEqualToZero();
            Property(x => x.LongerThanTen).MinimumLength(10);
            Property(x => x.NoMoreThanFiveCharacters).MaximumLength(5);
            Property(x => x.AtLeastFiveButNotTen).RangeLength(5, 10);
            Property(x => x.GreaterThanFive).MinValue(5);
            Property(x => x.LessThanFifteen).MaxValue(15d);
            Property(x => x.Email).Email();
            Property(x => x.Required).Required();

            Property(x => x.EmailCustomMessage).Email(CustomValidationKeys.CustomEmail);

            Property(x => x.Custom).Add<UniqueUsernameRule>();

            Property(x => x.Triggered).TriggeredValidation();
            Property(x => x.Triggered).GreaterThanZero();
            Property(x => x.Triggered).Required(ValidationMode.Live);
        }
    }
}