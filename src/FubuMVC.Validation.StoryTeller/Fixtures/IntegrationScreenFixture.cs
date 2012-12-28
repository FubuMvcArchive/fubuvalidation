using System.Linq;
using FubuMVC.Validation.Serenity;
using OpenQA.Selenium;
using Serenity.Fixtures;
using StoryTeller;
using StoryTeller.Engine;

namespace FubuMVC.Validation.StoryTeller.Fixtures
{
    public class IntegrationScreenFixture : ScreenFixture<IntegrationModel>
    {
        public IntegrationScreenFixture()
        {
            Title = "The Integration Model";
            EditableElementsForAllImmediateProperties();
        }

        protected override void beforeRunning()
        {
            Navigation.NavigateTo(new IntegrationModel());
        }

        private ValidationDriver validation
        {
            get { return new ValidationDriver(Driver, x => x.FindElement(By.Id("IntegrationModel"))); }
        }

        [FormatAs("There are no validation messages")]
        public bool NoMessages()
        {
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
            Driver.FindElement(By.Id("Model")).Click();
        }

        public IGrammar VerifyValidationMessages()
        {
            return VerifySetOf(() => validation.AllMessages())
                .Titled("Verify the validation messages")
                .MatchOn(x => x.Property, x => x.Message);
        }
    }
}