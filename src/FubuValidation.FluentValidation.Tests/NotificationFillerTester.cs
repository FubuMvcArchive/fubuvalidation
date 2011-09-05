using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using FubuCore.Reflection;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuValidation.FluentValidation.Tests
{
    [TestFixture]
    public class NotificationFillerTester : InteractionContext<NotificationFiller>
    {
        private Notification _notification;
        private ValidationResult _result;
        private NotificationMessage _message;

        protected override void beforeEach()
        {
            _notification = new Notification(typeof(SampleFluentModel));
            _result = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("RequiredField", "RequiredField is required") });
            _message = new NotificationMessage(ValidationKeys.REQUIRED);

            MockFor<INotificationMessageProvider>()
                .Expect(builder => builder.MessageFor(ReflectionHelper.GetAccessor<SampleFluentModel>(m => m.RequiredField), _result.Errors.First()))
                .Return(_message);
        }

        [Test]
        public void should_register_messages_for_errors()
        {
            ClassUnderTest
                .Fill(_notification, _result);

            _notification
                .AllMessages
                .ShouldHaveCount(1);
        }

        [Test]
        public void should_register_messages_for_accessors()
        {
            ClassUnderTest
                .Fill(_notification, _result);

            _notification
                .MessagesFor<SampleFluentModel>(m => m.RequiredField)
                .ShouldHaveCount(1);
        }

        [Test]
        public void should_build_string_tokens_through_token_builder()
        {
            ClassUnderTest
                .Fill(_notification, _result);

            VerifyCallsFor<INotificationMessageProvider>();
        }
    }
}