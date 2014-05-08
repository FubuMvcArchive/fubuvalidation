using FubuMVC.Core;
using FubuMVC.Core.Endpoints;
using FubuMVC.Katana;
using FubuMVC.StructureMap;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.IntegrationTesting.LoFi
{
    [TestFixture]
    public class IntegratedLoFiTester : ValidationHarness
    {
        private LoFiInput theInput;
        private EmbeddedFubuMvcServer theRuntime;

        [SetUp]
        public void SetUp()
        {
            theInput = new LoFiInput();

            var registry = new FubuRegistry();
            registry.Policies.Local.Add<ValidationPolicy>();
            registry.Actions.IncludeType<IntegratedLoFiEndpoint>();
            registry.Import<FubuMvcValidation>();

            theRuntime = FubuApplication.For(registry).StructureMap().RunEmbeddedWithAutoPort();
        }

        [TearDown]
        public void TearDown()
        {
            theRuntime.Dispose();
        }

        private HttpResponse theResponse
        {
            get { return theRuntime.Endpoints.PostAsForm(theInput); }
        }

        [Test]
        public void output_from_endpoint_if_validation_succeeds()
        {
            theInput.Name = "Josh";
            theResponse.ReadAsText().ShouldEqual(IntegratedLoFiEndpoint.SUCCESS);
        }

        [Test]
        public void redirects_to_get_if_validation_fails()
        {
            theInput.Name = null;
            theResponse.ReadAsText().ShouldEqual(IntegratedLoFiEndpoint.GET);
        }
    }
}