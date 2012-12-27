using FubuCore.Reflection;
using FubuMVC.Validation.Remote;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.Remote
{
    [TestFixture]
    public class RemoteFieldValidationRuleFilterTester
    {
        private RemoteFieldValidationRuleFilter theFilter;

        [SetUp]
        public void SetUp()
        {
            theFilter = new RemoteFieldValidationRuleFilter();
        }

        [Test]
        public void matches_rules_that_implment_IRemoteFieldValidationRule()
        {
            theFilter.Matches(new RemoteRuleStub()).ShouldBeTrue();
        }

        [Test]
        public void does_not_match_rules_that_do_not_implement_IRemoteFieldValidationRule()
        {
            theFilter.Matches(new RequiredFieldRule()).ShouldBeFalse();
        }

        public class RemoteRuleStub : IRemoteFieldValidationRule
        {
            public void Validate(Accessor accessor, ValidationContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}