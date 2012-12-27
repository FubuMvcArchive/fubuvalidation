using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Validation.Remote;

namespace FubuMVC.Validation
{
    [ConfigurationType(ConfigurationType.Discovery)]
    public class RegisterRemoteRuleQuery : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            var settings = graph.Settings.Get<ValidationSettings>();
            var query = new RemoteRuleQuery(settings.Filters);

            graph.Services.SetServiceIfNone(typeof(IRemoteRuleQuery), ObjectDef.ForValue(query));
        }
    }
}