using FubuMVC.Core.Ajax;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
	[TestFixture]
	public class ValidationAjaxExtensionsTester
	{
		[Test]
		public void gets_and_sets_the_validation_origin()
		{
			var continuation = new AjaxContinuation();

			continuation.ValidationOrigin(ValidationOrigin.Server);
			continuation.ValidationOrigin().ShouldEqual(ValidationOrigin.Server);

			continuation.ValidationOrigin(ValidationOrigin.Client);
			continuation.ValidationOrigin().ShouldEqual(ValidationOrigin.Client);
		}
	}
}