using System.Collections.Generic;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Validation.Registration;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.Registration
{
    [TestFixture]
    public class ValidationFailureExpressionTester
    {
        private List<ObjectDef> _policies;
        private ValidationFailureExpression _expression;

        [SetUp]
        public void before_each()
        {
            _policies = new List<ObjectDef>();
            _expression = new ValidationFailureExpression(_policies);
        }


        [Test]
        public void should_register_object_def_for_generic_type()
        {
            _expression.ApplyPolicy<TestPolicy>();
            _policies.ShouldContain(p => p.Type == typeof (TestPolicy));
        }

        [Test]
        public void should_register_object_def_for_type()
        {
            _expression.ApplyPolicy(typeof(TestPolicy));
            _policies.ShouldContain(p => p.Type == typeof(TestPolicy));
        }

        [Test]
        public void should_register_object_def_for_value()
        {
            var policy = new TestPolicy(null);
            _expression.ApplyPolicy(policy);
            _policies.ShouldContain(p => p.Value == policy);
        }

        public class TestPolicy : IValidationFailurePolicy
        {
            public TestPolicy(object dep)
            {    
            }

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