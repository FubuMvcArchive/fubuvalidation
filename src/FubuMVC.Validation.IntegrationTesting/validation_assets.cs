using System.Net;
using FubuMVC.Core.Assets;
using FubuMVC.Core.UI;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;

namespace FubuMVC.Validation.IntegrationTesting
{
    [TestFixture, Ignore]
    public class validation_assets
    {
        [Test]
        public void fetches_fubuvalidation_localization()
        {
            ServerHarness.Endpoints.Get<AssetEndpoint>(x => x.get_fubuvalidation_localization())
               .StatusCodeShouldBe(HttpStatusCode.OK)
               .ScriptNames()
               .ShouldHaveTheSameElementsAs("_content/scripts/jquery-1.8.2.min.js", "_content/scripts/underscore.min.js", "_content/scripts/fubuvalidation.localization.js");
        }
    }

    public class AssetEndpoint
    {
        private readonly FubuHtmlDocument _document;

        public AssetEndpoint(FubuHtmlDocument document)
        {
            _document = document;
        }

        public HtmlDocument get_fubuvalidation_localization()
        {
            _document.Asset("fubuvalidation.localization.js");
            _document.WriteAssetsToHead();

            return _document;
        }
    }
}