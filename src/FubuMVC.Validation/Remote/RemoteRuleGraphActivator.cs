using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bottles;
using Bottles.Diagnostics;
using FubuCore;
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
        private readonly IRemoteRuleQuery _remotes;
        private readonly ITypeDescriptorCache _properties;

        public RemoteRuleGraphActivator(ValidationGraph graph, RemoteRuleGraph remoteGraph, BehaviorGraph behaviorGraph,
                                        IRemoteRuleQuery remotes, ITypeDescriptorCache properties)
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
                .Each(FillRules);
        }

        public void FillRules(ActionCall call)
        {
            var input = call.InputType();
            if (input == null) return;

            getRules(input, new Type[0]).Each(x => _remoteGraph.RegisterRule(x.Item1, x.Item2));
        }

        private IEnumerable<Tuple<Accessor, IFieldValidationRule>> getRules(Type type, Type[] types)
        {
            var targetType = getTargetType(type);

            if (targetType == null || types.Contains(targetType))
            {
                yield break;
            }

            var inner = types.Concat(new[] {targetType}).ToArray();

            foreach (var property in _properties.GetPropertiesFor(targetType).Values)
            {
                Accessor accessor = new SingleProperty(property);
                var rules = _graph
                    .Query()
                    .RulesFor(accessor)
                    .Where(rule => _remotes.IsRemote(rule));
                foreach (var rule in rules)
                {
                    yield return Tuple.Create(accessor, rule);
                }
                foreach (var tuple in getRules(property.PropertyType, inner))
                {
                    yield return tuple;
                }
            }
        }
        private static Type getTargetType(Type type)
        {
            if (type == typeof (object))
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
                return getTargetType(type.FindParameterTypeTo(typeof (IEnumerable<>)));
            }
            return type;
        }
    }
}