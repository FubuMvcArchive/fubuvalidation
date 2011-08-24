using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class when_bootstrapping_validation
    {
        private BehaviorGraph _graph;

        [SetUp]
        public void setup_with_defaults()
        {
            _graph = new FubuRegistry(registry =>
                                          {
                                              registry
                                                  .Applies
                                                  .ToThisAssembly();

                                              registry
                                                  .Actions
                                                  .FindWith<SampleActionSource>();

                                              registry.Validation(validation =>
                                                                      {
                                                                          validation
                                                                              .Actions
                                                                              .Include(call => call.HasInput);

                                                                          validation
                                                                              .Failures
                                                                              .ApplyPolicy<SampleFailurePolicy>();
                                                                      });
                                          })
                        .BuildGraph();
        }

        [Test]
        public void should_register_field_source()
        {
            _graph
                .Services
                .ServicesFor<IValidationSource>()
                .First()
                .Value
                .ShouldBeOfType<FieldRuleSource>();
        }

        [Test]
        public void should_register_field_rules_registry()
        {
            _graph
                .Services
                .DefaultServiceFor<IFieldRulesRegistry>()
                .Value
                .ShouldBeOfType<FieldRulesRegistry>();
        }

        [Test]
        public void should_register_validator()
        {
            _graph
                .Services
                .DefaultServiceFor<IValidator>()
                .Type
                .ShouldEqual(typeof (Validator));
        }

        [Test]
        public void should_register_validation_continuation_handler()
        {
            _graph
                .Services
                .DefaultServiceFor<IValidationContinuationHandler>()
                .Type
                .ShouldEqual(typeof(ValidationContinuationHandler));
        }

        [Test]
        public void should_register_validation_failure_handler()
        {
            _graph
                .Services
                .DefaultServiceFor<IValidationFailureHandler>()
                .Type
                .ShouldEqual(typeof(ValidationFailureHandler));
        }

        [Test]
        public void should_register_policies()
        {
            _graph
                .Services
                .ServicesFor<IValidationFailurePolicy>()
                .ShouldHaveCount(1);
        }

        [Test]
        public void should_apply_behavior_to_actions_matching_predicate()
        {
            _graph
                .Behaviors
                .Where(chain => chain.OfType<ValidationNode>().Any())
                .ShouldHaveCount(1);
        }

        public class SampleActionSource : IActionSource
        {
            public IEnumerable<ActionCall> FindActions(TypePool types)
            {
                yield return ActionCall.For<SampleInputModel>(m => m.Test());
                yield return ActionCall.For<SampleInputModel>(m => m.Test("Hello"));
            }
        }

        public class SampleFailurePolicy : IValidationFailurePolicy
        {
            public bool Matches(ValidationFailure context)
            {
                throw new System.NotImplementedException();
            }

            public void Handle(ValidationFailure context)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}