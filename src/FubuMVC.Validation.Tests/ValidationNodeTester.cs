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
			new ValidationNode(null, null).Strategies.All().ShouldHaveTheSameElementsAs(RenderingStrategyRegistry.Default().All());
		}
	}
}