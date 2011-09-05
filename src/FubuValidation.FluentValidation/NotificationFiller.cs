using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation.Results;
using FubuCore.Reflection;

namespace FubuValidation.FluentValidation
{
    public class NotificationFiller : INotificationFiller
    {
        private readonly INotificationMessageProvider _messageProvider;

        public NotificationFiller(INotificationMessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
        }

        public void Fill(Notification notification, ValidationResult result)
        {
            result
                .Errors
                .Each(error =>
                          {
                              var accessor = buildAccessor(notification, error);
                              var message = _messageProvider.MessageFor(accessor, error);

                              notification.RegisterMessage(accessor, message);
                          });
        }

        private static Accessor buildAccessor(Notification notification, ValidationFailure failure)
        {
            // continuations should occur through FluentValidation itself
            var property = notification
                .TargetType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.Name == failure.PropertyName);

            if(property == null)
            {
                return null;
            }

            return new SingleProperty(property);
        }
    }
}