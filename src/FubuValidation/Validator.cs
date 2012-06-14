using System.Collections.Generic;
using System.Linq;
using FubuCore;

namespace FubuValidation
{
    public class Validator : IValidator
    {
        private readonly ITypeResolver _typeResolver;
        private readonly IValidationQuery _validationQuery;
        private readonly IServiceLocator _serviceLocator;

        public Validator(ITypeResolver typeResolver, IValidationQuery validationQuery,IServiceLocator serviceLocator)
        {
            _typeResolver = typeResolver;
            _validationQuery = validationQuery;
            _serviceLocator = serviceLocator;
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
            var context = new ValidationContext(this, notification, target){
                TargetType = validatedType,
                Resolver = _typeResolver,
                Services = _serviceLocator
            };

            _validationQuery
                .RulesFor(validatedType)
                .Each(rule => rule.Validate(context));
        }

        public static IValidator BasicValidator()
        {
            return BasicValidator(new InMemoryServiceLocator());
        }
        public static IValidator BasicValidator(IServiceLocator serviceLocator)
        {
            return new Validator(new TypeResolver(), ValidationQuery.BasicQuery(), serviceLocator);
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