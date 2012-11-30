using System.Net;
using FubuLocalization;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class AjaxValidationActionFilterTester : InteractionContext<AjaxValidationActionFilter<SampleInputModel>>
    {
        private SampleInputModel theInput;
        private Notification theNotification;
        private AjaxContinuation theAjaxContinuation;

        protected override void beforeEach()
        {
            theInput = new SampleInputModel();
            theNotification = new Notification();
            theAjaxContinuation = new AjaxContinuation();

            MockFor<IValidationFilter<SampleInputModel>>().Stub(x => x.Validate(theInput)).Return(theNotification);
            MockFor<IAjaxContinuationResolver>().Stub(x => x.Resolve(theNotification)).Return(theAjaxContinuation);
        }

        private FubuContinuation theContinuation { get { return ClassUnderTest.Validate(theInput); } }

        [Test]
        public void continues_to_next_behavior_when_validation_succeeds()
        {
            theContinuation.AssertWasContinuedToNextBehavior();
        }

        [Test]
        public void ends_with_500_when_validation_fails()
        {
            theNotification.RegisterMessage(StringToken.FromKeyString("Test"));
            theContinuation.AssertWasEndedWithStatusCode(HttpStatusCode.InternalServerError);
        }

        [Test]
        public void writes_the_ajax_continuation_when_validation_fails()
        {
            theNotification.RegisterMessage(StringToken.FromKeyString("Test"));
            ClassUnderTest.Validate(theInput);

            MockFor<IJsonWriter>().AssertWasCalled(x => x.Write(theAjaxContinuation.ToDictionary(), MimeType.Json.Value));
        }
    }
}