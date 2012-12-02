using System.Net;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class AjaxValidationFailureHandlerTester : InteractionContext<AjaxValidationFailureHandler>
    {
        private AjaxContinuation theAjaxContinuation;
        private Notification theNotification;

        protected override void beforeEach()
        {
            theNotification = new Notification();
            theAjaxContinuation = new AjaxContinuation();

            MockFor<IAjaxContinuationResolver>().Stub(x => x.Resolve(theNotification)).Return(theAjaxContinuation);
            ClassUnderTest.Handle(theNotification);
        }

        [Test]
        public void writes_bad_request_status_code()
        {
            MockFor<IOutputWriter>().AssertWasCalled(x => x.WriteResponseCode(HttpStatusCode.BadRequest));
        }

        [Test]
        public void sets_the_continuation_in_the_request()
        {
            MockFor<IFubuRequest>().AssertWasCalled(x => x.Set(theAjaxContinuation));
        }
    }
}