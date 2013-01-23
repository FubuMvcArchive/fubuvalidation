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
                .ShouldHaveTheSameElementsAs(ValidationNode.DefaultFor(ValidationMode.Ajax));
			
            new ValidationActionFilter(null, null)
                .Validation
                .ShouldHaveTheSameElementsAs(ValidationNode.DefaultFor(ValidationMode.LoFi));
		}
	}
}