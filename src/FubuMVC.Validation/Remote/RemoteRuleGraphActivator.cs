using System.Collections.Generic;
using System.Linq;
using Bottles;
using Bottles.Diagnostics;
using FubuCore.Reflection;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation.Remote
{
    public class RemoteRuleGraphActivator : IActivator
    {
        private readonly ValidationGraph _graph;
        private readonly RemoteRuleGraph _remoteGraph;
        private readonly BehaviorGraph _behaviorGraph;
        private readonly ITypeDescriptorCache _properties;

        public RemoteRuleGraphActivator(ValidationGraph graph, RemoteRuleGraph remoteGraph, BehaviorGraph behaviorGraph, ITypeDescriptorCache properties)
        {
            _graph = graph;
            _remoteGraph = remoteGraph;
            _behaviorGraph = behaviorGraph;
            _properties = properties;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            // Find the input models that have remote rules
            // "Bake" them into the remote graph
            _behaviorGraph
                .Actions()
                .Each(FillRules);
        }

        public void FillRules(ActionCall call)
        {
            var input = call.InputType();
            if (input == null) return;

            _properties.ForEachProperty(input, property =>
            {
                var accessor = new SingleProperty(property);
                var rules = _graph
                    .Query()
                    .RulesFor(accessor)
                    .Where(rule => rule.GetType().HasAttribute<RemoteAttribute>()); // TODO -- Do better than a friggin attribute

                rules.Each(rule => _remoteGraph.RegisterRule(accessor, rule));
            });
        }
    }
}