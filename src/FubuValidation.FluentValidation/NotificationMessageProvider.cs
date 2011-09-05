using FluentValidation.Results;
using FubuCore;
using FubuCore.Reflection;
using FubuLocalization;

namespace FubuValidation.FluentValidation
{
    public class NotificationMessageProvider : INotificationMessageProvider
    {
        public NotificationMessage MessageFor(Accessor accessor, ValidationFailure failure)
        {
            var token = StringToken.FromKeyString("{0}.{1}".ToFormat(accessor.OwnerType.Name, accessor.Name),
                                                  failure.ErrorMessage);
            return new NotificationMessage(token);
        }
    }
}