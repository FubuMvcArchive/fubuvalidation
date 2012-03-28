using System.Linq;
using FubuTestingSupport;
using FubuValidation.Fields;
using FubuValidation.Tests.Models;
using NUnit.Framework;

namespace FubuValidation.Tests.Fields
{
    [TestFixture]
    public class when_validating_minimum_length
    {
        private UserModel theModel;
        private MinimumLengthRule theRule;

        [SetUp]
        public void BeforeEach()
        {
            theRule = new MinimumLengthRule(10);
            theModel = new UserModel();
        }

        [Test]
        public void should_register_message_if_value_is_null()
        {
            theModel.Password = null;
            theRule.ValidateProperty(theModel, x => x.Password).AllMessages.Any().ShouldBeTrue();
        }

        [Test]
        public void should_register_message_if_value_is_whitespace()
        {
            theModel.Password = "          ";
            theRule.ValidateProperty(theModel, x => x.Password).AllMessages.Any().ShouldBeTrue();
        }

        [Test]
        public void should_register_message_if_length_is_less_than_required()
        {
            theModel.Password = "password";
            theRule.ValidateProperty(theModel, x => x.Password).AllMessages.Any().ShouldBeTrue();
        }

        [Test]
        public void should_not_register_message_if_length_is_equal_required()
        {
            theModel.Password = "password12";
            theRule.ValidateProperty(theModel, x => x.Password).AllMessages.Any().ShouldBeFalse();
        }

        [Test]
        public void should_not_register_message_if_length_is_greater_than_required()
        {
            theModel.Password = "password13";
            theRule.ValidateProperty(theModel, x => x.Password).AllMessages.Any().ShouldBeFalse();
        }
    }
}