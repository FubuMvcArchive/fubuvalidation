using FluentValidation.Results;
using FubuCore;
using FubuCore.Reflection;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuValidation.FluentValidation.Tests
{
    [TestFixture]
    public class NotificationMessageProviderTester
    {
        private NotificationMessageProvider _provider;
        private Accessor _accessor;
        private ValidationFailure _failure;
        private NotificationMessage _message;

        [SetUp]
        public void beforeEach()
        {
            _provider = new NotificationMessageProvider();
            _accessor = ReflectionHelper.GetAccessor<SampleFluentModel>(m => m.RequiredField);
            _failure = new ValidationFailure(_accessor.Name, "RequiredField is required");

            _message = _provider.MessageFor(_accessor, _failure);
        }

        [Test]
        public void should_set_token_key()
        {
            _message
                .StringToken
                .Key
                .ShouldEqual("{0}.{1}".ToFormat(typeof(SampleFluentModel).Name, _accessor.Name));
        }

        [Test]
        public void should_set_token_default_value()
        {
            _message
                .StringToken
                .DefaultValue
                .ShouldEqual(_failure.ErrorMessage);
        }
    }
}