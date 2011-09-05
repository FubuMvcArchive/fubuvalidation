using System;
using System.Collections.Generic;
using System.Linq;
using Fluent = FluentValidation;

namespace FubuValidation.FluentValidation
{
    public class FluentValidationSource : IValidationSource
    {
        private readonly IEnumerable<Fluent.IValidator> _validators;
        private readonly INotificationFiller _notificationFiller;

        public FluentValidationSource(IEnumerable<Fluent.IValidator> validators, INotificationFiller notificationFiller)
        {
            _validators = validators;
            _notificationFiller = notificationFiller;
        }

        public IEnumerable<IValidationRule> RulesFor(Type type)
        {
            var matchingValidators = _validators
                .Where(v => v.CanValidateInstancesOfType(type))
                .ToList();

            if(!matchingValidators.Any())
            {
                yield break;
            }

            yield return new FluentValidationRule(matchingValidators, _notificationFiller);
        }
    }
}