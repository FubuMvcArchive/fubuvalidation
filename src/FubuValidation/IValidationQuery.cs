using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore.Reflection;
using FubuValidation.Fields;

namespace FubuValidation
{
    public interface IValidationQuery
    {
        IEnumerable<IValidationRule> RulesFor<T>();
        IEnumerable<IValidationRule> RulesFor(Type type);
    }

    public class ValidationQuery : IValidationQuery
    {
        private readonly IEnumerable<IValidationSource> _sources;

        public ValidationQuery(IEnumerable<IValidationSource> sources)
        {
            _sources = new List<IValidationSource>(sources){
                new SelfValidatingClassRuleSource()
            };
        }

        public IEnumerable<IValidationRule> RulesFor<T>()
        {
            return RulesFor(typeof (T));
        }

        public IEnumerable<IValidationRule> RulesFor(Type type)
        {
            return _sources.SelectMany(src => src.RulesFor(type));
        }

        public static ValidationQuery BasicQuery()
        {
            return new ValidationQuery(new IValidationSource[]{
                new FieldRuleSource(new FieldRulesRegistry(new IFieldValidationSource[]{new AttributeFieldValidationSource() }, new TypeDescriptorCache()))
            });
        }
    }
}