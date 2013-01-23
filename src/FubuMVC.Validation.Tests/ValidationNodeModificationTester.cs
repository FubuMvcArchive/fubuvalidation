using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Policies;
using FubuMVC.Validation.Tests.UI;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
	[TestFixture]
	public class ValidationNodeModificationTester
	{
		[Test]
		public void matches()
		{
			var filter = new LambdaChainFilter(x => true);
			new ValidationNodeModification(filter, null).Matches(new BehaviorChain()).ShouldBeTrue();
		}

		[Test]
		public void matches_negative()
		{
			var filter = new LambdaChainFilter(x => false);
			new ValidationNodeModification(filter, null).Matches(new BehaviorChain()).ShouldBeFalse();
		}

		[Test]
		public void modifies_the_validation_node()
		{
			var modification = new ValidationNodeModification(null, x => x.Clear());
			var node = new AjaxValidationNode(ActionCall.For<FormValidationModeEndpoint>(x => x.post_ajax(null)));
			
			var chain = new BehaviorChain();
			chain.AddToEnd(node);

			modification.Modify(chain);

			node.ShouldHaveCount(0);
		}
	}
}