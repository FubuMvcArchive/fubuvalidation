using FubuCore.Reflection;
using FubuLocalization;
using FubuTestingSupport;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuValidation.Tests
{
    [TestFixture]
    public class IsValidTester
    {
        private ValidationContext theContext;
        private Accessor theAccessor;

        [SetUp]
        public void SetUp()
        {
            theAccessor = ReflectionHelper.GetAccessor<IsValidTarget>(x => x.Name);
            theContext = ValidationContext.For(new object());
        }

        [Test]
        public void invalid()
        {
            theContext.Notification.RegisterMessage(theAccessor, StringToken.FromKeyString("Test", "Test"));
            new IsValid().Matches(theAccessor, theContext).ShouldBeFalse();
        }

        [Test]
        public void valid()
        {
            var otherAccessor = ReflectionHelper.GetAccessor<IsValidTarget>(x => x.Other);
            theContext.Notification.RegisterMessage(otherAccessor, StringToken.FromKeyString("Test", "Test"));
            new IsValid().Matches(theAccessor, theContext).ShouldBeTrue();
        }

        public class IsValidTarget
        {
            public string Name { get; set; }
            public string Other { get; set; }
        }
    }
}