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
            var response = makePost();
            response.StatusCode.ShouldEqual(HttpStatusCode.Redirect);
        }

        // TODO -- Open this up in FubuMVC.TestingHarness
        private HttpResponse makePost()
        {
            var dictionary = new Dictionary<string, object>();
            new TypeDescriptorCache().ForEachProperty(typeof(LoFiInput), prop =>
            {
                var rawValue = prop.GetValue(theInput, null);
                var httpValue = rawValue == null ? string.Empty : rawValue.ToString().UrlEncoded();

                dictionary.Add(prop.Name, httpValue);
            });

            var post = dictionary.Select(x => "{0}={1}".ToFormat(x.Key, x.Value)).Join("&");
            WebRequest request = WebRequest.Create(Urls.UrlFor(theInput, "POST"));
            request.ContentType = "application/x-www-form-urlencoded";

            request.Method = "POST";

            var httpRequest = request.As<HttpWebRequest>();
            httpRequest.Accept = "*/*";
            httpRequest.AllowAutoRedirect = false;

            var stream = request.GetRequestStream();
            var bytes = Encoding.Default.GetBytes(post);
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            return request.ToHttpCall();
        }
    }
}