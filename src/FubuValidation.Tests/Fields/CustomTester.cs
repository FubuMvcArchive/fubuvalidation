using System.Linq;
using FubuTestingSupport;
using FubuValidation.Fields;
using FubuValidation.Tests.Models;
using NUnit.Framework;

namespace FubuValidation.Tests.Fields
{
    [TestFixture]
    public class CustomTester
    {
        private SimpleModel theModel;
        private CustomFieldValidationRule<SimpleModel> theRule;

        [SetUp]
        public void BeforeEach()
        {
            theModel = new SimpleModel();
            theRule = new CustomFieldValidationRule<SimpleModel>(x => (x.Number + 1) /2 == 7);
        }

        [Test]
        public void should_not_register_any_messages()
        {
            theModel.Number = 14;

            theRule.ValidateProperty(theModel, x => x.Number)
                   .MessagesFor<SimpleModel>(x => x.Number)
                   .Select(x => x.StringToken)
                   .Any().ShouldBeFalse();
        }

        [Test]
        public void should_have_message_when_expression_is_false()
        {
            theModel.Number = 12;

            theRule.ValidateProperty(theModel, x => x.Number)
                   .MessagesFor<SimpleModel>(x => x.Number)
                   .Select(x => x.StringToken)
                   .ShouldHaveTheSameElementsAs(ValidationKeys.InvalidFormat);
        }

        [Test]
        public void should_have_custom_message()
        {
            var customToken = new ValidationKeys("That is some bad math.");
            theModel.Number = 12;
            theRule = new CustomFieldValidationRule<SimpleModel>(x => (x.Number + 1) /2 == 7, customToken);

            theRule.ValidateProperty(theModel, x => x.Number)
                   .MessagesFor<SimpleModel>(x => x.Number)
                   .Select(x => x.StringToken)
                   .ShouldHaveTheSameElementsAs(customToken);
        }
    }
}