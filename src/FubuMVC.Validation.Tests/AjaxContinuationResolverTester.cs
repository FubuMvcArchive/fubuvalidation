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
        public void should_decorate_continuation()
        {
            var notification = new Notification();
            var continuation = new AjaxContinuation();

            MockFor<IAjaxContinuationDecorator>()
                .Expect(d => d.Enrich(Arg<AjaxContinuation>.Is.NotNull, Arg<Notification>.Is.Same(notification)))
                .Return(continuation);

            ClassUnderTest
                .Resolve(notification);

            VerifyCallsFor<IAjaxContinuationDecorator>();
        }
    }
}