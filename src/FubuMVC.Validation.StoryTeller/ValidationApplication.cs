using FubuMVC.Core;
using FubuMVC.Core.UI;
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
                Actions.IncludeType<SelectTagEndpoint>();
                Actions.IncludeType<ClassValidationEndpoint>();

                Import<HtmlConventionRegistry>(x => x.Editors.IfPropertyIs<SimpleList>().BuildBy<SimpleListBuilder>());

                AlterSettings<ValidationSettings>(x => x.Remotes.Include<UniqueUsernameRule>());
            }
        }
    }
}