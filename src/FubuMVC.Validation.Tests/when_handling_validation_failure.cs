using System;
using System.Collections.Generic;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;
using StructureMap.AutoMocking;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class when_handling_validation_failure
    {
        private ValidationFailureHandler _handler;
        private ValidationFailure _context;
        private List<IValidationFailurePolicy> _policies;

        [SetUp]
        public void SetUp()
        {
            var services = new RhinoAutoMocker<SampleInputModel>(MockMode.AAA);
            var request = services.Get<IFubuRequest>();

            _context = new ValidationFailure(ActionCall.For<SampleInputModel>(m => m.Test("Hello")), Notification.Valid(), "Hello");
            _policies = new List<IValidationFailurePolicy>();
            _handler = new ValidationFailureHandler(_policies, request);
        }

        [Test]
        public void should_throw_validation_exception_if_no_policies_are_found()
        {
            Exception<FubuMVCValidationException>
                .ShouldBeThrownBy(() => _handler.Handle(_context));
        }

        [Test]
        public void should_invoke_first_policy_that_is_matched()
        {
            bool firstPolicyInvoked = false;
            _policies.Add(new SampleValidationFailurePolicy(() => { firstPolicyInvoked = true;  }));
            _policies.Add(new SampleValidationFailurePolicy(() => Assert.Fail("Invalid policy invoked")));

            _handler.Handle(_context);

            firstPolicyInvoked.ShouldBeTrue();
        }

        public class SampleValidationFailurePolicy : IValidationFailurePolicy
        {
            private readonly Action _continuation;

            public SampleValidationFailurePolicy(Action continuation)
            {
                _continuation = continuation;
            }

            public bool Matches(ValidationFailure context)
            {
                return true;
            }

            public void Handle(ValidationFailure context)
            {
                _continuation();
            }
        }
    }
}