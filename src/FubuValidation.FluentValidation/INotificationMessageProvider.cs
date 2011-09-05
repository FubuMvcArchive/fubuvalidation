using FluentValidation.Results;
using FubuCore.Reflection;

namespace FubuValidation.FluentValidation
{
    public interface INotificationMessageProvider
    {
        NotificationMessage MessageFor(Accessor accessor, ValidationFailure failure);
    }
}