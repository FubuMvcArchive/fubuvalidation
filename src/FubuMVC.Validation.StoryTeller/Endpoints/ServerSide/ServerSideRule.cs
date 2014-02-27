using FubuCore.Reflection;
using FubuLocalization;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation.StoryTeller
{
    public class ServerSideRule : IFieldValidationRule
    {
        public ServerSideRule()
        {
            Token = ValidationKeys.InvalidFormat;
        }

        public StringToken Token { get; set; }

        public ValidationMode Mode { get; set; }

        public void Validate(Accessor accessor, ValidationContext context)
        {
            context.Notification.RegisterMessage(accessor, Token);
        }
    }
}