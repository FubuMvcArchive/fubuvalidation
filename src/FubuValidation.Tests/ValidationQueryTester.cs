using System;
using System.Collections.Generic;
using FubuTestingSupport;
using FubuValidation.Fields;
using FubuValidation.Tests.Models;
using NUnit.Framework;

namespace FubuValidation.Tests
{
    [TestFixture]
    public class ValidationQueryTester
    {
        private ValidationQuery _query;

        [SetUp]
        public void setup()
        {
            _query = new ValidationQuery(new[]
                                             {
                                                 new StubbedValidationSource(type => type.Equals(typeof(SimpleModel)), new[] { new ClassFieldValidationRules() }),
                                                 new StubbedValidationSource(type => type.Equals(typeof(SimpleModel)), new[] { new StubbedValidationRule() })
                                             });
        }

        [Test]
        public void should_find_rules_from_sources()
        {
            _query
                .RulesFor<SimpleModel>()
                .ShouldHaveCount(2);

            _query
                .RulesFor<CompositeModel>()
                .ShouldHaveCount(0);
        }

        [Test]
        public void should_register_self_validating_class_source_by_default()
        {
            _query
                .RulesFor<ValidatingModel>()
                .ShouldHaveCount(1);
        }

        public class ValidatingModel : IValidated
        {
            public void Validate(ValidationContext context)
            {
            }
        }

        public class StubbedValidationRule : IValidationRule
        {
            public void Validate(ValidationContext context)
            {
            }
        }

        public class StubbedValidationSource : IValidationSource
        {
            private readonly Func<Type, bool> _predicate;
            private readonly IEnumerable<IValidationRule> _rules;

            public StubbedValidationSource(Func<Type, bool> predicate, IEnumerable<IValidationRule> rules)
            {
                _predicate = predicate;
                _rules = rules;
            }

            public IEnumerable<IValidationRule> RulesFor(Type type)
            {
                if (!_predicate(type)) return new IValidationRule[0];
                return _rules;
            }
        }
    }
}