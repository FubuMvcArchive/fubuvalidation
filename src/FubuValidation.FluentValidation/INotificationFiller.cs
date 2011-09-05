using FluentValidation.Results;

namespace FubuValidation.FluentValidation
{
    public interface INotificationFiller
    {
        void Fill(Notification notification, ValidationResult result);
    }
}