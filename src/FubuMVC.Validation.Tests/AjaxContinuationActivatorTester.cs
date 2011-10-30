using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class AjaxContinuationActivatorTester
    {
        private AjaxContinuationActivator _activator;

        [SetUp]
        public void before_each()
        {
            _activator = new AjaxContinuationActivator();
        }

        [Test]
        public void should_never_be_null()
        {
            // just a sanity check
            _activator
                .Activate(new Notification())
                .ShouldNotBeNull();
        }
    }
}