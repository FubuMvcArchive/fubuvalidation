using System.Linq;
using FubuCore;
using FubuMVC.Core.Registration.Nodes;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class ValidationConventionTester
    {
        [Test]
        public void adds_validation_action_filter_for_lofi_endpoints()
        {
            var call = ActionCall.For<SampleInputModel>(x => x.Test(null));
            
            var chain = new BehaviorChain();
            chain.AddToEnd(call);

            ValidationConvention.ApplyValidation(call);

            var nodes = chain.ToArray();
            var filter = nodes[0].As<ActionFilter>();
            filter.HandlerType.ShouldEqual(typeof (ValidationActionFilter<string>));
        }

        [Test]
        public void adds_ajax_validation_action_filter_for_ajax_endpoints()
        {
            var call = ActionCall.For<SampleAjaxModel>(x => x.post_model(null));

            var chain = new BehaviorChain();
            chain.AddToEnd(call);

            ValidationConvention.ApplyValidation(call);

            var nodes = chain.ToArray();
            nodes[0].ShouldBeOfType<AjaxValidationNode>();
        }
    }
}