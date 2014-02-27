using FubuValidation;

namespace FubuMVC.Validation.StoryTeller
{
    public class ServerSideRules : ClassValidationRules<ServerSideModel>
    {
        public ServerSideRules()
        {
            Property(x => x.Value).Rule<ServerSideRule>();
        }
    }
}