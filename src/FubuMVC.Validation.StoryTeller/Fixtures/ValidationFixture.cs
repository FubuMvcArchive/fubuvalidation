using FubuMVC.HelloValidation.Handlers.Testing;
using OpenQA.Selenium;
using Serenity;
using Serenity.Fixtures;
using StoryTeller;
using StoryTeller.Engine;

namespace FubuMVC.Validation.StoryTeller.Fixtures
{
    public class ValidationFixture : ScreenFixture<TestItem>
    {
        private ValidationSummaryDriver _summaryDriver;

        public ValidationFixture()
        {
            Title = "Validation in the application";
            EditableElementsForAllImmediateProperties();
        }

        protected override void beforeRunning()
        {
            Navigation.NavigateTo(new TestItem());

            _summaryDriver = new ValidationSummaryDriver(Driver);
        }

        private IWebElement form
        {
            get { return Driver.FindElement(By.Id(typeof(TestItem).Name)); }
        }

        private IWebElement summary
        {
            get { return form.FindElement(By.CssSelector(".validation-container")); }
        }

        [FormatAs("Click the 'Submit' button")]
        public void ClickSubmit()
        {
            form.FindElement(By.Id("Create")).Click();
            Wait.Until(() => summary.Displayed);
        }

        public IGrammar VerifyMessages()
        {
            return VerifySetOf(() => _summaryDriver.AllValidationMessages())
                .Titled("The validation messages are")
                .MatchOn(x => x.Property, x => x.Message);
        }
    }
}