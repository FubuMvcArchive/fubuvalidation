using FubuMVC.Core.Ajax;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class AjaxContinuationResolverTester : InteractionContext<AjaxContinuationResolver>
    {
        [Test]
        public void should_construct_through_continuation_activator()
        {
            var notification = new Notification();
            var continuation = new AjaxContinuation();

            MockFor<IAjaxContinuationActivator>()
                .Expect(a => a.Activate(notification))
                .Return(continuation);

            // just here to trick the container
            MockFor<IAjaxContinuationDecorator>()
                .Expect(d => d.Enrich(continuation, notification))
                .Return(continuation);

            ClassUnderTest
                .Resolve(notification)
                .ShouldEqual(continuation);
        }

        [Test]
        public void should_decorate_continuation()
        {
            var notification = new Notification();
            var continuation = new AjaxContinuation();

            MockFor<IAjaxContinuationActivator>()
                .Expect(a => a.Activate(notification))
                .Return(continuation);

            MockFor<IAjaxContinuationDecorator>()
                .Expect(d => d.Enrich(continuation, notification))
                .Return(continuation);

            ClassUnderTest
                .Resolve(notification);

            VerifyCallsFor<IAjaxContinuationDecorator>();
        }
    }
}