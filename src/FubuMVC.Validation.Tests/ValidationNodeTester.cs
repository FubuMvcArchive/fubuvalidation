using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Validation.Tests.UI;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
	[TestFixture]
	public class ValidationNodeTester
	{
		[Test]
		public void default_rendering_strategies()
		{
			new AjaxValidationNode(ActionCall.For<FormValidationModeEndpoint>(x => x.post_ajax(null))).Strategies.All().ShouldHaveTheSameElementsAs(RenderingStrategyRegistry.Default().All());
			new ValidationActionFilter(null, null).Strategies.All().ShouldHaveTheSameElementsAs(RenderingStrategyRegistry.Default().All());
		}
	}
}