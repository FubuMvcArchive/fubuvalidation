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
            new AjaxValidationNode(ActionCall.For<FormValidationModeEndpoint>(x => x.post_ajax(null)))
                .Validation
                .ShouldHaveTheSameElementsAs(ValidationNode.Default());
			
            new ValidationActionFilter(null, null)
                .Validation
                .ShouldHaveTheSameElementsAs(ValidationNode.Default());
		}

		[Test]
		public void is_empty()
		{
			ValidationNode.Empty().IsEmpty().ShouldBeTrue();
		}

		[Test]
		public void is_empty_negative()
		{
			ValidationNode.Default().IsEmpty().ShouldBeFalse();
		}
	}
}