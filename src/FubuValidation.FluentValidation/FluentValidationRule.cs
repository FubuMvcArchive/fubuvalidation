using System.Collections.Generic;
using System.Linq;

namespace FubuValidation.FluentValidation
{
    public class FluentValidationRule : IValidationRule
    {
        private readonly IEnumerable<global::FluentValidation.IValidator> _validators;
        private readonly INotificationFiller _notificationFiller;

        public FluentValidationRule(IEnumerable<global::FluentValidation.IValidator> validators, INotificationFiller notificationFiller)
        {
            _validators = validators;
            _notificationFiller = notificationFiller;
        }

        public IEnumerable<global::FluentValidation.IValidator> Validators
        {
            get { return _validators; }
        }

        public void Validate(ValidationContext context)
        {
            _validators
                .Select(v => v.Validate(context.Target))
                .Each(result => _notificationFiller.Fill(context.Notification, result));
        }
    }
}