using System;
using System.Diagnostics;
using System.Linq;
using FubuCore.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Ajax;
using FubuMVC.Katana;
using FubuMVC.StructureMap;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.IntegrationTesting.Ajax
{
    [TestFixture]
    public class IntegratedAjaxTester : ValidationHarness
    {
        private AjaxRequest theRequest;
        private EmbeddedFubuMvcServer theRuntime;

        [SetUp]
        public void SetUp()
        {
            theRequest = new AjaxRequest();

            var registry = new FubuRegistry();
            registry.Policies.Local.Add<ValidationPolicy>();
            registry.Actions.IncludeType<IntegratedAjaxEndpoint>();
            registry.Import<FubuMvcValidation>();
            theRuntime = FubuApplication.For(registry).StructureMap().RunEmbeddedWithAutoPort();
        }

        [TearDown]
        public void TearDown()
        {
            theRuntime.Dispose();
        }

        private JsonResponse theContinuation
        {
            get
            {
                var response = theRuntime.Endpoints.PostJson(theRequest);

                try
                {
                    return response.ReadAsJson<JsonResponse>();
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
            theContinuation.success.ShouldBeTrue();
        }

        [Test]
        public void validation_error_for_name()
        {
            theRequest.Name = null;
            var errors = theContinuation.errors;

            errors.ShouldHaveCount(1);
            errors.Any(x => x.field == ReflectionHelper.GetAccessor<AjaxRequest>(r => r.Name).Name).ShouldBeTrue();
        }  
    }

    public class JsonResponse
    {
        public bool success { get; set; }
        public AjaxError[] errors { get; set; }
    }
}