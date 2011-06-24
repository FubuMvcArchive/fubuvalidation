using System;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
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
        protected override void beforeEach()
        {
            _continuation = FubuContinuation.NextBehavior();
            Container
                .Configure(x =>
                {
                    x.For<Func<Type, bool>>().Use(t => t == typeof(SampleInputModel));
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
        }

        [Test]
        public void should_match_on_predicate()
        {
            ClassUnderTest
                .Matches(typeof (SampleInputModel))
                .ShouldBeTrue();

            ClassUnderTest
                .Matches(typeof(string))
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
                .Handle(typeof(SampleInputModel), Notification.Valid());

            VerifyCallsFor<IFubuRequest>();
            VerifyCallsFor<IActionBehavior>();
        }
    }
}