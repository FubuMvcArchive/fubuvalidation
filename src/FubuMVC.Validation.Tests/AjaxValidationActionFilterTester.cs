using System.Net;
using FubuLocalization;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class AjaxValidationBehaviorTester : InteractionContext<AjaxValidationBehavior<SampleInputModel>>
    {
        private SampleInputModel theInput;
        private Notification theNotification;
        private AjaxContinuation theAjaxContinuation;

        protected override void beforeEach()
        {
            theInput = new SampleInputModel();
            theNotification = new Notification();
            theAjaxContinuation = new AjaxContinuation();

            ClassUnderTest.InsideBehavior = MockFor<IActionBehavior>();
            MockFor<IFubuRequest>().Stub(x => x.Get<SampleInputModel>()).Return(theInput);
            MockFor<IValidationFilter<SampleInputModel>>().Stub(x => x.Validate(theInput)).Return(theNotification);
            MockFor<IAjaxContinuationResolver>().Stub(x => x.Resolve(theNotification)).Return(theAjaxContinuation);
        }

        [Test]
        public void continues_to_next_behavior_when_validation_succeeds()
        {
            ClassUnderTest.Invoke();
            MockFor<IActionBehavior>().AssertWasCalled(x => x.Invoke());
        }

        [Test]
        public void writes_the_ajax_continuation_when_validation_fails()
        {
            theNotification.RegisterMessage(StringToken.FromKeyString("Test"));
            ClassUnderTest.Invoke();

            MockFor<IAjaxValidationFailureHandler>().AssertWasCalled(x => x.Handle(theNotification));
        }
    }
}