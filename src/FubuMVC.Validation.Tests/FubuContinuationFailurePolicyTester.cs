using System;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class FubuContinuationFailurePolicyTester : InteractionContext<FubuContinuationFailurePolicy>
    {
        private FubuContinuation _continuation;
        private ValidationFailure _context;

        protected override void beforeEach()
        {
            _continuation = FubuContinuation.NextBehavior();
            _context = new ValidationFailure(ActionCall.For<SampleInputModel>(m => m.Test(1)),
                                                         Notification.Valid(), 1);
            Container
                .Configure(x =>
                {
                    x.For<Func<ValidationFailure, bool>>().Use(ctx => ctx.InputType() == typeof(int));
                    x.For<FubuContinuation>().Use(_continuation);
                });

            MockFor<IFubuContinuationResolver>()
                .Expect(r => r.Resolve(_context))
                .Return(_continuation);
        }

        [Test]
        public void should_match_on_predicate()
        {
            ClassUnderTest
                .Matches(_context)
                .ShouldBeTrue();

            var context = new ValidationFailure(ActionCall.For<SampleInputModel>(m => m.Test("Hello")), Notification.Valid(), "Hello");

            ClassUnderTest
                .Matches(context)
                .ShouldBeFalse();
        }

        [Test]
        public void should_invoke_continuation_handler()
        {
            MockFor<IFubuRequest>()
                .Expect(r => r.Set(_continuation));

            MockFor<IValidationContinuationHandler>()
                .Expect(handler => handler.Handle());

            ClassUnderTest
                .Handle(_context);

            VerifyCallsFor<IFubuRequest>();
            VerifyCallsFor<IValidationContinuationHandler>();
        }
    }
}