using System.Linq;
using FubuMVC.Validation.Serenity;
using OpenQA.Selenium;
using Serenity.Fixtures;
using StoryTeller;
using StoryTeller.Engine;

namespace FubuMVC.Validation.StoryTeller
{
    public class CreateUserScreenFixture : ScreenFixture<CreateUser>
    {
        public CreateUserScreenFixture()
        {
            Title = "The 'Create User' Screen";
            EditableElementsForAllImmediateProperties();
        }

        protected override void beforeRunning()
        {
            Navigation.NavigateTo(new CreateUser());
        }

        private ValidationDriver validation
        {
            get { return new ValidationDriver(Driver, x => x.FindElement(By.Id("CreateUserForm"))); }
        }

        [FormatAs("Click the 'Create User' button")]
        public void ClickCreate()
        {
            Driver.FindElement(By.Id("CreateUser")).Click();
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

        public IGrammar VerifyValidationMessages()
        {
            return VerifySetOf(() => validation.AllMessages())
                .Titled("Verify the validation messages")
                .MatchOn(x => x.Property, x => x.Message);
        }
    }
}