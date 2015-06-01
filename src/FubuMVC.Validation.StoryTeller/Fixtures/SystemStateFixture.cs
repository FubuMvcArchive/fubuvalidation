using FubuMVC.Validation.StoryTeller.Endpoints.User;
using StoryTeller;
using StoryTeller.Grammars.Tables;

namespace FubuMVC.Validation.StoryTeller.Fixtures
{
    public class SystemStateFixture : Fixture
    {
        private IUserService _users;

        public SystemStateFixture()
        {
            Title = "The system state is";
        }

        public override void SetUp()
        {
            _users = Retrieve<IUserService>();

            _users.Clear();
        }

        [FormatAs("There are no users")]
        public void NoUsers()
        {
            // just a marker
        }

        public IGrammar TheUsersAre()
        {
            return CreateNewObject<User>("The users are", x =>
            {
                x.SetProperty(u => u.Username);
                x.Do = user => _users.Update(user);
            }).AsTable("The Users are");
        }
    }
}