using System.Globalization;
using FubuLocalization;
using FubuMVC.Core.Ajax;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class StandardAjaxContinuationDecoratorTester
    {
        private StandardAjaxContinuationDecorator _decorator;
        private Notification _notification;

        [SetUp]
        public void before_each()
        {
			LocalizationManager.Stub();

            _notification = new Notification(typeof(SampleInputModel));
            _decorator = new StandardAjaxContinuationDecorator();

            _notification.RegisterMessage<SampleInputModel>(m => m.Field, StringToken.FromKeyString("Field", "Message"));
        }


        [Test]
        public void should_add_corresponding_errors()
        {
            continuation()
                .Errors
                .ShouldHaveCount(1);
        }

        [Test]
        public void should_set_field_for_errors()
        {
            continuation()
                .Errors
                .ShouldContain(e => e.field.Equals("Field"));
        }

		[Test]
		public void sets_the_label()
		{
			continuation()
				.Errors
				.ShouldContain(e => e.label.Equals("en-US_Field"));
		}

        [Test]
        public void should_set_messages_for_errors()
        {
            continuation()
                .Errors
                .ShouldContain(e => e.message.Equals("Message"));
        }

        [Test]
        public void should_set_success_flag()
        {
            continuation()
                .Success
                .ShouldBeFalse();

            _notification = new Notification(typeof(SampleInputModel));

            continuation()
                .Success
                .ShouldBeTrue();
        }

        private AjaxContinuation continuation()
        {
            return _decorator.Enrich(new AjaxContinuation(), _notification);
        }
    }
}