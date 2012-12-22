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
                .StructureMapObjectFactory();
        }

        public class ValidationStoryTellerRegistry : FubuRegistry
        {
            public ValidationStoryTellerRegistry()
            {
                Actions.IncludeType<CreateUserEndpoint>();
            }
        }
    }
}