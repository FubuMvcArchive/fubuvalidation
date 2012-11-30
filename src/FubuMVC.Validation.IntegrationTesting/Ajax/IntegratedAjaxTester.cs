using System.Diagnostics;
using System.Linq;
using FubuCore.Reflection;
using FubuMVC.Core.Ajax;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.IntegrationTesting.Ajax
{
    [TestFixture]
    public class IntegratedAjaxTester : ValidationHarness
    {
        private AjaxRequest theRequest;

        [SetUp]
        public void SetUp()
        {
            theRequest = new AjaxRequest();
        }

        protected override void configure(Core.FubuRegistry registry)
        {
            registry.Actions.IncludeType<IntegratedAjaxEndpoint>();
            registry.Import<FubuValidation>();
        }

        private AjaxContinuation theContinuation
        {
            get
            {
                var response = endpoints.PostJson(theRequest);
                try
                {
                    return response.ReadAsJson<AjaxContinuation>();
                }
                catch
                {
                    Debug.WriteLine(response.ReadAsText());
                    throw;
                }
                
            }
        }

        [Test]
        public void validation_passes()
        {
            theRequest.Name = "Josh";
            theContinuation.Success.ShouldBeTrue();
        }

        [Test]
        public void validation_error_for_name()
        {
            theRequest.Name = null;
            var errors = theContinuation.Errors;

            errors.ShouldHaveCount(1);
            errors.Any(x => x.field == ReflectionHelper.GetAccessor<AjaxRequest>(r => r.Name).Name).ShouldBeTrue();
        }
    }
}