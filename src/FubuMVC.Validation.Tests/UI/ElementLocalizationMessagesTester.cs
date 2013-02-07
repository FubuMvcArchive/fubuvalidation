using FubuMVC.Validation.UI;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.UI
{
	[TestFixture]
	public class LocalizationAnnotationStrategyTester
	{
		private LocalizationAnnotationStrategy theStrategy;

		[SetUp]
		public void SetUp()
		{
			theStrategy = new LocalizationAnnotationStrategy();
		}

		[Test]
		public void matches_when_token_is_not_null()
		{
			var theRule = new RequiredFieldRule();
			theStrategy.Matches(theRule).ShouldBeTrue();
		}

		[Test]
		public void no_match_when_token_is_null()
		{
			var theRule = new RequiredFieldRule();
			theRule.Token = null;
			theStrategy.Matches(theRule).ShouldBeFalse();
		}
	}


	[TestFixture]
	public class ElementLocalizationMessagesTester
	{
		private ElementLocalizationMessages theMessages;

		[SetUp]
		public void SetUp()
		{
			theMessages = new ElementLocalizationMessages();
		}

		[Test]
		public void adds_the_rules()
		{
			theMessages.Add(new RequiredFieldRule());
			theMessages.Add(new EmailFieldRule());

			var messages = theMessages.Messages;

			messages["required"].ShouldEqual(ValidationKeys.Required.ToString());
			messages["email"].ShouldEqual(ValidationKeys.Email.ToString());
		}
	}
}