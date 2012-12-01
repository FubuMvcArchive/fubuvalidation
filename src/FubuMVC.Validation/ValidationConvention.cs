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
            IValidationFilterBuilder builder;
            if(call.HasOutput && call.OutputType().CanBeCastTo<AjaxContinuation>())
            {
                builder = typeof (AjaxValidationFilterBuilder<>).CloseAndBuildAs<IValidationFilterBuilder>(call.InputType());
            }
            else
            {
                builder = typeof (LoFiValidationFilterBuilder<>).CloseAndBuildAs<IValidationFilterBuilder>(call.InputType());
            }

            call.AddBefore(builder.FilterFor(call));
        }

        public interface IValidationFilterBuilder
        {
            ActionFilter FilterFor(ActionCall call);
        }

        public class LoFiValidationFilterBuilder<T> : IValidationFilterBuilder where T : class
        {
            public ActionFilter FilterFor(ActionCall call)
            {
                return ActionFilter.For<ValidationActionFilter<T>>(x => x.Validate(null));
            }
        }

        public class AjaxValidationFilterBuilder<T> : IValidationFilterBuilder where T : class
        {
            public ActionFilter FilterFor(ActionCall call)
            {
                return ActionFilter.For<AjaxValidationActionFilter<T>>(x => x.Validate(null));
            }
        }
    }
}