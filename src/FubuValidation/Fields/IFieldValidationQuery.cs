using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FubuCore.Reflection;

namespace FubuValidation.Fields
{
    public interface IFieldValidationQuery
    {
        IEnumerable<IFieldValidationRule> RulesFor<T>(Expression<Func<T, object>> property);
        IEnumerable<IFieldValidationRule> RulesFor(Accessor accessor);
        bool HasRule<T>(Accessor accessor) where T : IFieldValidationRule;
        void ForRule<T>(Accessor accessor, Action<T> continuation) where T : IFieldValidationRule;
    }

    public class FieldValidationQuery : IFieldValidationQuery
    {
        private readonly IFieldRulesRegistry _registry;

        public FieldValidationQuery(IFieldRulesRegistry registry)
        {
            _registry = registry;
        }

        public IEnumerable<IFieldValidationRule> RulesFor<T>(Expression<Func<T, object>> property)
        {
            return RulesFor(property.ToAccessor());
        }

        public IEnumerable<IFieldValidationRule> RulesFor(Accessor accessor)
        {
            var chain = accessor as PropertyChain;
            if (chain == null)
            {
                return _registry.RulesFor(accessor.OwnerType).RulesFor(accessor);
            }

            if(chainHasValidationContinuedProperties(chain))
            {
                var prop = chain.InnerProperty;
                return _registry.RulesFor(prop.ReflectedType).RulesFor(new SingleProperty(prop));
            }

            return new IFieldValidationRule[0];
        }

        private bool chainHasValidationContinuedProperties(PropertyChain chain)
        {
            var propertyValueGetters = chain.ValueGetters.OfType<PropertyValueGetter>().ToArray();

            var accessors = propertyValueGetters
                .Take(propertyValueGetters.Length - 1)
                .Select(p => new SingleProperty(p.PropertyInfo))
                .ToList();

            return accessors.Any() && accessors.All(HasRule<ContinuationFieldRule>);
        }

        public bool HasRule<T>(Accessor accessor) where T : IFieldValidationRule
        {
            return getRules<T>(accessor).Any();
        }

        public void ForRule<T>(Accessor accessor, Action<T> continuation) where T : IFieldValidationRule
        {
            getRules<T>(accessor)
                .Each(continuation);
        }

        private IEnumerable<T> getRules<T>(Accessor accessor)
            where T : IFieldValidationRule
        {
            return RulesFor(accessor)
                .OfType<T>();
        }
    }
}