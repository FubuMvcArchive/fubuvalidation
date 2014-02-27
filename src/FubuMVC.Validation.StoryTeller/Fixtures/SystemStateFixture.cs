using FubuMVC.Validation.StoryTeller.Endpoints.User;
using StoryTeller;
using StoryTeller.Engine;

namespace FubuMVC.Validation.StoryTeller.Fixtures
{
    public class SystemStateFixture : Fixture
    {
        private IUserService _users;

        public SystemStateFixture()
        {
            Title = "The system state is";
        }

        public override void SetUp(ITestContext context)
        {
            _users = context.Retrieve<IUserService>();

            _users.Clear();
        }

        [FormatAs("There are no users")]
        public void NoUsers()
        {
            // just a marker
        }

        public IGrammar TheUsersAre()
        {
            return CreateNewObject<User>(x =>
            {
                x.SetProperty(u => u.Username);
                x.Do = user => _users.Update(user);
            }).AsTable("The Users are");
        }
    }
}