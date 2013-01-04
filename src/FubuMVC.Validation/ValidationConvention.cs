using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Validation
{
    [ConfigurationType(ConfigurationType.InjectNodes)]
    public class ValidationConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            var settings = graph.Settings.Get<ValidationSettings>();
            var filter = settings.As<IApplyValidationFilter>();

            graph
                .Actions()
                .Where(chain => filter.Filter(chain.ParentChain()))
                .Each(ApplyValidation);
        }

        public static void ApplyValidation(ActionCall call)
        {
            BehaviorNode node;
            if(call.ResourceType().CanBeCastTo<AjaxContinuation>())
            {
                node = new AjaxValidationNode(call);
            }
            else
            {
                var builder = typeof (LoFiValidationNodeBuilder<>).CloseAndBuildAs<IValidationNodeBuilder>(call.InputType());
                node = builder.BuildNode();
            }

			call.AddBefore(node);
        }

        public interface IValidationNodeBuilder
        {
            BehaviorNode BuildNode();
        }

        public class LoFiValidationNodeBuilder<T> : IValidationNodeBuilder where T : class
        {
            public BehaviorNode BuildNode()
            {
				return ValidationActionFilter.ValidationFor<ValidationActionFilter<T>>(x => x.Validate(null));
            }
        }
    }
}