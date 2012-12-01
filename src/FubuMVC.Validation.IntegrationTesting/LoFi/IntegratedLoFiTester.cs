using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Endpoints;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.IntegrationTesting.LoFi
{
    [TestFixture]
    public class IntegratedLoFiTester : ValidationHarness
    {
        private LoFiInput theInput;

        [SetUp]
        public void SetUp()
        {
            theInput = new LoFiInput();
        }

        protected override void configure(Core.FubuRegistry registry)
        {
            registry.Actions.IncludeType<IntegratedLoFiEndpoint>();
            registry.Import<FubuValidation>();
        }

        private HttpResponse theResponse
        {
            get { return endpoints.PostAsForm(theInput); }
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