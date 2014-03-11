using FubuMVC.Core.Endpoints;
using FubuMVC.Validation.IntegrationTesting.LoFi_diff_models;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.IntegrationTesting.LoFi
{
    [TestFixture]
    public class IntegratedLoFi_diff_models_Tester : ValidationHarness
    {
        private LoFiInput_post postInputModel;

        [SetUp]
        public void SetUp()
        {
            postInputModel = new LoFiInput_post();
        }

        protected override void configure(Core.FubuRegistry registry)
        {
            registry.Actions.IncludeType<IntegratedLoFi_diff_models_Endpoint>();
            registry.Import<FubuMvcValidation>();
        }

        private HttpResponse theResponse
        {
            get { return endpoints.PostAsForm(postInputModel); }
        }

        [Test]
        public void output_from_endpoint_if_validation_succeeds()
        {
            postInputModel.Name = "Josh";
            theResponse.ReadAsText().ShouldEqual(IntegratedLoFi_diff_models_Endpoint.SUCCESS);
        }

        [Test]
        public void redirects_to_get_if_validation_fails()
        {
            postInputModel.Name = null;
            theResponse.ReadAsText().ShouldEqual(IntegratedLoFi_diff_models_Endpoint.GET);
        }
    }
}