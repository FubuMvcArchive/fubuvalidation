using System;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
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
        private ValidationFailureContext _context;

        protected override void beforeEach()
        {
            _continuation = FubuContinuation.NextBehavior();
            _context = new ValidationFailureContext(ActionCall.For<SampleInputModel>(m => m.Test(1)),
                                                         Notification.Valid(), 1);
            Container
                .Configure(x =>
                {
                    x.For<Func<ValidationFailureContext, bool>>().Use(ctx => ctx.InputType() == typeof(int));
                    x.For<FubuContinuation>().Use(_continuation);

                    // MockFor blowing up on ctor for ContinuationHandler otherwise
                    x.For<ContinuationHandler>().Use(ctx =>
                                                         {
                                                             var handler =
                                                                 new ContinuationHandler(
                                                                     ctx.GetInstance<IUrlRegistry>(),
                                                                     ctx.GetInstance<IOutputWriter>(),
                                                                     ctx.GetInstance<IFubuRequest>(),
                                                                     ctx.GetInstance<IPartialFactory>());
                                                             handler.InsideBehavior = ctx.GetInstance<IActionBehavior>();
                                                             return handler;
                                                         });
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

            var context = new ValidationFailureContext(ActionCall.For<SampleInputModel>(m => m.Test("Hello")), Notification.Valid(), "Hello");

            ClassUnderTest
                .Matches(context)
                .ShouldBeFalse();
        }

        [Test]
        public void should_invoke_continuation_handler()
        {
            MockFor<IFubuRequest>()
                .Expect(r => r.Get<FubuContinuation>())
                .Return(_continuation);

            MockFor<IFubuRequest>()
                .Expect(r => r.Set(_continuation));

            MockFor<IActionBehavior>()
                .Expect(b => b.Invoke());

            ClassUnderTest
                .Handle(_context);

            VerifyCallsFor<IFubuRequest>();
            VerifyCallsFor<IActionBehavior>();
        }
    }
}