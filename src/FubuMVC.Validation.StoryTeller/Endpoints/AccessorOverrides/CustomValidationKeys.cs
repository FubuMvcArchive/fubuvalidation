using FubuLocalization;

namespace FubuMVC.Validation.StoryTeller.Endpoints.AccessorOverrides
{
    public class CustomValidationKeys : StringToken
    {
        public static readonly CustomValidationKeys CustomEmail = new CustomValidationKeys("Custom email error");

        protected CustomValidationKeys(string text)
            : base(null, text, namespaceByType: true)
        {
        }
    }
}