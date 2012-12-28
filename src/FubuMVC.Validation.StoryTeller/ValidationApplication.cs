using FubuMVC.Core;
using FubuMVC.StructureMap;

namespace FubuMVC.Validation.StoryTeller
{
    public class ValidationApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            return FubuApplication
                .For<ValidationStoryTellerRegistry>()
                .StructureMapObjectFactory(x => x.ForSingletonOf<IUserService>().Use<UserService>());
        }

        public class ValidationStoryTellerRegistry : FubuRegistry
        {
            public ValidationStoryTellerRegistry()
            {
                Actions.IncludeType<CreateUserEndpoint>();
                Actions.IncludeType<IntegrationEndpoint>();

                AlterSettings<ValidationSettings>(x => x.Remotes.Include<UniqueUsernameRule>());
            }
        }
    }
}