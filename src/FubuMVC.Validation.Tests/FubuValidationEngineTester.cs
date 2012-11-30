using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                    .Actions
                    .FindWith<SampleActionSource>();


                registry.Import<FubuValidation>();


            });
        }

        #endregion

        private BehaviorGraph _graph;


        public class SampleActionSource : IActionSource
        {
            public IEnumerable<ActionCall> FindActions(Assembly assembly)
            {
                yield return ActionCall.For<SampleInputModel>(m => m.Test());
                yield return ActionCall.For<SampleInputModel>(m => m.Test("Hello"));
            }
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
    }
}