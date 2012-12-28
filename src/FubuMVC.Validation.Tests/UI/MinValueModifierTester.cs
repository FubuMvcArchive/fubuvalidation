using FubuMVC.Core.UI.Elements;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class MinValueModifierTester : ValidationElementModifierContext<MinValueModifier>
    {
        [Test]
        public void adds_the_min_data_attribute_for_min_value_rule()
        {
            var theRequest = ElementRequest.For(new TargetWithMinValue(), x => x.Value);
            tagFor(theRequest).Data("min").ShouldEqual(10);
        }

        [Test]
        public void no_data_attribute_when_rule_does_not_exist()
        {
            var theRequest = ElementRequest.For(new TargetWithNoMinValue(), x => x.Value);
            tagFor(theRequest).Data("min").ShouldBeNull();
        }


        public class TargetWithMinValue
        {
            [MinValue(10)]
            public string Value { get; set; }
        }

        public class TargetWithNoMinValue
        {
            public string Value { get; set; }
        }
    }
}