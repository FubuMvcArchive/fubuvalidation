using FubuCore.Reflection;
using FubuMVC.Validation.Remote;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.Remote
{
    [TestFixture]
    public class RemoteRuleAttributeFilterTester
    {
        private RemoteRuleAttributeFilter theFilter;

        [SetUp]
        public void SetUp()
        {
            theFilter = new RemoteRuleAttributeFilter();
        }

        [Test]
        public void matches_rules_with_the_remote_attribute()
        {
            theFilter.Matches(new RuleWithRemoteAttribute()).ShouldBeTrue();
        }

        [Test]
        public void does_not_match_rules_without_the_rule_attribute()
        {
            theFilter.Matches(new RequiredFieldRule()).ShouldBeFalse();
        }

        [Remote]
        public class RuleWithRemoteAttribute : IFieldValidationRule
        {
            public void Validate(Accessor accessor, ValidationContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}