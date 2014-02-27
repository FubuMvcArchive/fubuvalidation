using System.Linq;
using FubuMVC.Validation.Serenity;
using FubuMVC.Validation.StoryTeller.Endpoints.User;
using OpenQA.Selenium;
using Serenity;
using Serenity.Fixtures;
using StoryTeller;
using StoryTeller.Engine;

namespace FubuMVC.Validation.StoryTeller.Fixtures
{
    public class CreateUserScreenFixture : ScreenFixture<User>
    {
        public CreateUserScreenFixture()
        {
            Title = "The 'Create User' Screen";
            EditableElementsForAllImmediateProperties();
        }

        private IUserService _users;

        protected override void beforeRunning()
        {
            Navigation.NavigateTo(new User());
            _users = Retrieve<IUserService>();
        }

        private ValidationDriver validation
        {
            get { return new ValidationDriver(Driver, x => x.FindElement(By.Id("CreateUserForm"))); }
        }

        [FormatAs("Click the 'Create User' button")]
        public void ClickCreate()
        {
            Driver.FindElement(By.Id("User")).Click();
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

        public IGrammar VerifyTheUsers()
        {
            return VerifySetOf(() =>
                {
                    Wait.Until(() => _users.All().Any());
                    return _users.All();
                })
                .Titled("Verify the users")
                .MatchOn(x => x.Username);
        }

        [FormatAs("Verify there are no users")]
        public bool NoUsers()
        {
            return !_users.All().Any();
        }
    }
}