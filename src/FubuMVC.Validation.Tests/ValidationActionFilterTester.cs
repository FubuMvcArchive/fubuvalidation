using FubuLocalization;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class ValidationActionFilterTester : InteractionContext<ValidationActionFilter<ActionFilterTarget>>
    {
        private ActionFilterTarget theTarget;
        private Notification theNotification;

        protected override void beforeEach()
        {
            theTarget = new ActionFilterTarget();
            theNotification = new Notification();
            MockFor<IValidationFilter<ActionFilterTarget>>().Stub(x => x.Validate(theTarget)).Return(theNotification);
        }

        private FubuContinuation theContinuation { get { return ClassUnderTest.Validate(theTarget); } }

        [Test]
        public void continues_to_next_behavior_when_validation_succeeds()
        {
            theContinuation.AssertWasContinuedToNextBehavior();
        }

        [Test]
        public void transfers_to_GET_category_of_input_type_when_validation_fails()
        {
            theNotification.RegisterMessage(StringToken.FromKeyString("Test"));
            theContinuation.AssertWasTransferedTo(theTarget, "GET");
        }

        [Test]
        public void sets_the_notification_in_the_request()
        {
            theNotification.RegisterMessage(StringToken.FromKeyString("Test"));
            ClassUnderTest.Validate(theTarget);
            MockFor<IFubuRequest>().AssertWasCalled(x => x.Set(theNotification));
        }
    }

    public class ActionFilterTarget
    {
    }
}