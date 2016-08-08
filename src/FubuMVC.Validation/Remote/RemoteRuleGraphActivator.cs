using System;
using System.Collections.Generic;
using System.Linq;
using Bottles;
using Bottles.Diagnostics;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core.Registration;
using FubuValidation;

namespace FubuMVC.Validation.Remote
{
    public class RemoteRuleGraphActivator : IActivator
    {
        private readonly ValidationGraph _graph;
        private readonly RemoteRuleGraph _remoteGraph;
        private readonly BehaviorGraph _behaviorGraph;
        private readonly IRemoteRuleQuery _remotes;
        private readonly ITypeDescriptorCache _properties;

        public RemoteRuleGraphActivator(ValidationGraph graph, RemoteRuleGraph remoteGraph, BehaviorGraph behaviorGraph, IRemoteRuleQuery remotes, ITypeDescriptorCache properties)
        {
            _graph = graph;
            _remoteGraph = remoteGraph;
            _behaviorGraph = behaviorGraph;
            _remotes = remotes;
            _properties = properties;
        }
        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            // Find the input models that have remote rules
            // "Bake" them into the remote graph
            _behaviorGraph
                .Actions()
                .Where(x => x.HasInput)
                .Select(x => x.InputType())
                .Distinct()
                .Each(fillRules);
        }

        private void fillRules(Type type)
        {
            type = targetType(type);
            if (type == null)
            {
                return;
            }
            _properties.ForEachProperty(type, property =>
            {
                var accessor = new SingleProperty(property);
                var rules = _graph
                    .Query()
                    .RulesFor(accessor)
                    .Where(rule => _remotes.IsRemote(rule));

                rules.Each(rule => _remoteGraph.RegisterRule(accessor, rule));
                if (property.PropertyType == type)
                {
                    return;
                }
                fillRules(property.PropertyType);
            });
        }

        private static Type targetType(Type type)
        {
            if (type == typeof(object))
            {
                return null;
            }
            if (type.IsPrimitive)
            {
                return null;
            }
            if (type.IsNullable())
            {
                return null;
            }
            if (type.IsValueType)
            {
                return null;
            }
            if (type.IsString())
            {
                return null;
            }
            if (type.IsGenericEnumerable())
            {
                return targetType(type.FindParameterTypeTo(typeof(IEnumerable<>)));
            }

            return type;
        }
    }
}