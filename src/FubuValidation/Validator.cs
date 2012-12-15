using System.Collections.Generic;
using System.Linq;
using FubuCore;

namespace FubuValidation
{
    public class Validator : IValidator
    {
        private readonly ITypeResolver _typeResolver;
        private readonly IValidationQuery _validationQuery;
        private readonly IServiceLocator _services;

        public Validator(ITypeResolver typeResolver, IValidationQuery validationQuery, IServiceLocator services)
        {
            _typeResolver = typeResolver;
            _validationQuery = validationQuery;
            _services = services;
        }

        public Notification Validate(object target)
        {
            var validatedType = _typeResolver.ResolveType(target);
            var notification = new Notification(validatedType);
            Validate(target, notification);
            return notification;
        }

        public void Validate(object target, Notification notification)
        {
            var validatedType = _typeResolver.ResolveType(target);
            var context = new ValidationContext(this, notification, target) {
                TargetType = validatedType,
                Resolver = _typeResolver,
                ServiceLocator = _services
            };

            _validationQuery
                .RulesFor(validatedType)
                .Each(rule => rule.Validate(context));
        }

        public static IValidator BasicValidator()
        {
            return new Validator(new TypeResolver(), ValidationQuery.BasicQuery(), new InMemoryServiceLocator());
        }

        public static Notification ValidateObject(object target)
        {
            return BasicValidator().Validate(target);
        }

        public static IEnumerable<NotificationMessage> ValidateField(object target, string propertyName)
        {
            var notification = ValidateObject(target);
            return notification.AllMessages.Where(x => x.Accessors.Any(a => a.Name == propertyName));
        }
            
    }
}