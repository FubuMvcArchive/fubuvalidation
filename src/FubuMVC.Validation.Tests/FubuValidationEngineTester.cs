using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{


    [TestFixture]
    public class when_bootstrapping_validation
    {
        #region Setup/Teardown

        [SetUp]
        public void setup_with_defaults()
        {
            _graph = BehaviorGraph.BuildFrom(registry =>
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


            });
        }

        #endregion

        private BehaviorGraph _graph;


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
                throw new NotImplementedException();
            }

            public void Handle(ValidationFailure context)
            {
                throw new NotImplementedException();
            }
        }



        [Test]
        public void should_apply_behavior_to_actions_matching_predicate()
        {
            _graph
                .Behaviors
                .Where(chain => chain.OfType<ValidationNode>().Any())
                .ShouldHaveCount(1);
        }

        [Test]
        public void should_register_policies()
        {
            _graph
                .Services
                .ServicesFor<IValidationFailurePolicy>()
                .ShouldHaveCount(2);
        }

        [Test]
        public void should_register_validation_continuation_handler()
        {
            _graph
                .Services
                .DefaultServiceFor<IValidationContinuationHandler>()
                .Type
                .ShouldEqual(typeof (ValidationContinuationHandler));
        }

        [Test]
        public void should_register_the_default_model_binding_errors()
        {
            _graph
                .Services
                .DefaultServiceFor<IModelBindingErrors>()
                .Type
                .ShouldEqual(typeof(ModelBindingErrors));
        }

        [Test]
        public void should_register_validation_failure_handler()
        {
            _graph
                .Services
                .DefaultServiceFor<IValidationFailureHandler>()
                .Type
                .ShouldEqual(typeof (ValidationFailureHandler));
        }
    }
}