using FubuMVC.Core;
using FubuMVC.Core.UI;
using FubuMVC.StructureMap;
using FubuMVC.Validation.StoryTeller.Endpoints.User;
using FubuMVC.Validation.StoryTeller.Endpoints.ValidationElementHandler;
using FubuMVC.Validation.UI;

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
                Actions.IncludeClassesSuffixedWithEndpoint();

                Import<HtmlConventionRegistry>(x => x.Editors.IfPropertyIs<SimpleList>().BuildBy<SimpleListBuilder>());
                AlterSettings<ValidationSettings>(validation =>
                {
                    validation.Remotes.Include<UniqueUsernameRule>();
                    validation.Remotes.Include<ThrottledRule>();

                    //validation.ForInputType<InlineModel>(x => x.RegisterStrategy(RenderingStrategies.Inline));
                    validation.Import<CustomizeValidation>();
                });
            }
        }
    }

    public class CustomizeValidation : ValidationSettingsRegistry
    {
        public CustomizeValidation()
        {
            //ForChainsMatching<CustomChainFilter>(x => ...);
            ForInputTypesMatching(x => x.Name.Contains("Inline"), x => x.RegisterStrategy(RenderingStrategies.Inline));
        }
    }
}