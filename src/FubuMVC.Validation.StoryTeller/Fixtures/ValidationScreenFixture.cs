using System.Linq;
using System.Threading;
using FubuCore;
using FubuMVC.Validation.Serenity;
using OpenQA.Selenium;
using Serenity;
using Serenity.Fixtures;
using StoryTeller;
using StoryTeller.Engine;

namespace FubuMVC.Validation.StoryTeller.Fixtures
{
	public abstract class ValidationScreenFixture<T> : ScreenFixture<T> where T : class, new()
	{
		protected ValidationScreenFixture()
		{
			Title = "The {0}".ToFormat(typeof (T).Name);
			EditableElementsForAllImmediateProperties();
		}

		protected override void beforeRunning()
		{
			Navigation.NavigateTo(new T());
		}

		private ValidationDriver validation
		{
			get { return new ValidationDriver(Driver, x => x.FindElement(By.Id(typeof(T).Name))); }
		}

		[FormatAs("There are no validation messages")]
		public bool NoMessages()
		{
			Wait.Until(() => validation.Hidden);
			return !validation.AllMessages().Any();
		}

		[FormatAs("The validation summary is not shown")]
		public bool NoSummary()
		{
			return validation.Hidden;
		}

		[FormatAs("Click the submit button")]
		public void ClickTheSubmitButton()
		{
			click();
			click();
			click();
		}

		private void click()
		{
			try
			{
				Driver.FindElement(By.Id("Model")).Click();
			}
			catch
			{
			}
			finally
			{
				Thread.Sleep(100);
			}
			
		}

		public IGrammar VerifyValidationMessages()
		{
			return VerifySetOf(() => validation.AllMessages())
				.Titled("Verify the validation messages")
				.MatchOn(x => x.Property, x => x.Message);
		}

		public IGrammar VerifyInlineValidationMessages()
		{
			return VerifySetOf(() => validation.InlineMessages())
				.Titled("Verify the inline validation messages")
				.MatchOn(x => x.Property, x => x.Message);
		}
	}
}