using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime.Conditionals;
using FubuMVC.Core.View;

namespace FubuMVC.Validation.UI
{
    [ConfigurationType(ConfigurationType.Instrumentation)]
    public class AttachDefaultValidationSummary : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            var chain = graph.BehaviorFor(typeof (ValidationSummary));
            if (chain == null) return;

            if (!chain.Output.HasView(typeof(Always)))
            {
                chain.Output.Writers.AddToEnd(new DefaultValidationSummaryNode());
            }
        }
    }
}