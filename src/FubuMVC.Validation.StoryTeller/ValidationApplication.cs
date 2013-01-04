using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.UI;
using FubuMVC.StructureMap;
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
                AlterSettings<ValidationSettings>(x => x.Remotes.Include<UniqueUsernameRule>());

				Policies.Add<MyRenderingStrategies>();
            }
        }

		[ConfigurationType(ConfigurationType.Instrumentation)]
		public class MyRenderingStrategies : IConfigurationAction
		{
			public void Configure(BehaviorGraph graph)
			{
				var chain = graph.BehaviorFor<InlineModelEndpoint>(e => e.post_inline_model(null));
				var validation = chain.ValidationNode();
				if(validation != null)
				{
					validation.Strategies.RegisterStrategy(RenderingStrategy.Inline);
				}
			}
		}
    }
}