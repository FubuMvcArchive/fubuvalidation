using System;
using System.Collections.Generic;
using System.Reflection;
using FubuCore;
using FubuCore.Reflection;
using FubuValidation.Fields;

namespace FubuValidation.Tests
{
    public class ValidationScenario<T>
    {
        private ValidationScenario(ScenarioDefinition definition)
        {
            var validator = new Validator(new TypeResolver(), definition.Query(), definition.Services);

            Model = definition.Model;
            Notification = validator.Validate(Model);
        }

        public T Model { get; set; }

        public Notification Notification { get; set; }

        public static ValidationScenario<T> For(Action<ScenarioDefinition> configuration)
        {
            var scenario = new ScenarioDefinition();
            configuration(scenario);

            return new ValidationScenario<T>(scenario);
        }

        public class ScenarioDefinition
        {
            private readonly IList<IValidationRule> _rules = new List<IValidationRule>();
            private readonly IList<IValidationSource> _sources;
            private readonly InMemoryServiceLocator _services = new InMemoryServiceLocator();
            private readonly IList<IFieldValidationRule> _fieldRules = new List<IFieldValidationRule>(); 

            public ScenarioDefinition()
            {
                _sources = new List<IValidationSource> { new PassThruValidationSource(_rules) };

                var fieldSource = new PassThruFieldValidationSource(_fieldRules);
                var registry = new FieldRulesRegistry(new IFieldValidationSource[] { fieldSource } , new TypeDescriptorCache());
                _sources.Add(new FieldRuleSource(registry));
            }

            public T Model { get; set; }
            
            public IServiceLocator Services
            {
                get { return _services; }
            }

            public void Service<TService>(TService service)
            {
                _services.Add(service);
            }

            public void ValidationSource<T>(T source)
                where T : IValidationSource
            {
                _sources.Add(source);
            }

            public void Rule<T>(T rule)
                where T : IValidationRule
            {
                _rules.Add(rule);
            }

            public void FieldRule<T>(T rule)
                where T : IFieldValidationRule
            {
                _fieldRules.Add(rule);
            }

            public IValidationQuery Query()
            {
                return new ValidationQuery(_sources);
            }
        }

        public class PassThruValidationSource : IValidationSource
        {
            private readonly IEnumerable<IValidationRule> _rules;

            public PassThruValidationSource(IEnumerable<IValidationRule> rules)
            {
                _rules = rules;
            }

            public IEnumerable<IValidationRule> RulesFor(Type type)
            {
                return _rules;
            }
        }

        public class PassThruFieldValidationSource : IFieldValidationSource
        {
            private readonly IEnumerable<IFieldValidationRule> _rules;

            public PassThruFieldValidationSource(IEnumerable<IFieldValidationRule> rules)
            {
                _rules = rules;
            }

            public IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
            {
                return _rules;
            }

            public void Validate()
            {
            }
        }
    }
}