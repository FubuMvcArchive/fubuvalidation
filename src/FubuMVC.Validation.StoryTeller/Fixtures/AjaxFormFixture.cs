using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Serenity.Fixtures;
using StoryTeller;
using StoryTeller.Engine;

namespace FubuMVC.Validation.StoryTeller.Fixtures
{
	// TODO -- Come back and add a test to use this after we figure out the Bottles issue
	public class AjaxFormFixture : ScreenFixture<AjaxModel>
	{
		public AjaxFormFixture()
		{
			Title = "Ajax Form";

			EditableElementsForAllImmediateProperties();
		}

		protected override void beforeRunning()
		{
			Navigation.NavigateTo(new AjaxModel());
		}

		[FormatAs("Submit the form")]
		public void Submit()
		{
			Driver.FindElement(By.Id("Submit")).Click();
		}

		[FormatAs("No messages were recorded")]
		public bool NoMessages()
		{
			return !messages().Any();
		}

		public IGrammar VerifyTheMessages()
		{
			return VerifySetOf(messages)
				.Titled("Verify the messages")
				.MatchOn(x => x.Message);
		}

		private IEnumerable<RecordedMessage> messages()
		{
			return Driver.InjectJavascript<IEnumerable<object>>("return AjaxController.allMessages()")
				.Cast<string>()
				.Select(x => new RecordedMessage { Message = x});
		}

		public class RecordedMessage
		{
			public string Message { get; set; }
		}
	}
}