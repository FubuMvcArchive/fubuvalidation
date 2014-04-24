using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Runtime.Conditionals;

namespace FubuMVC.Validation.UI
{
    public class AttachDefaultValidationSummary : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            var chain = graph.BehaviorFor(typeof (ValidationSummary));
            if (chain == null) return;

            if (!chain.Output.HasView(Always.Flyweight))
            {
                chain.Output.Add(new DefaultValidationSummaryWriter());
            }
        }
    }
}