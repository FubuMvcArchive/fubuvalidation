using System.Collections.Generic;
using System.Net;
using FubuMVC.Core.Assets;
using FubuMVC.Core.UI;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;

namespace FubuMVC.Validation.IntegrationTesting
{
    [TestFixture]
    public class validation_assets
    {
        [SetUp]
        public void SetUp()
        {
            ServerHarness.Start();
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void fetches_fubuvalidation()
        {
            var scripts = new List<string>
            {
                "_content/scripts/fubucontinuations.js",
                "_content/scripts/jquery-1.8.2.min.js",
                "_content/scripts/underscore-min.js",
                "_content/scripts/fubucontinuations.diagnostics.js",
                "_content/scripts/fubuvalidation.localization.js",
                "_content/scripts/jquery.form.js",
                "_content/scripts/fubucontinuations.jquery.js",
                "_content/scripts/fubuvalidation.rules.js",
                "_content/scripts/fubucontinuations.jquery.forms.js",
                "_content/scripts/fubuvalidation.core.js",
                "_content/scripts/fubuvalidation.ui.js",
                "_content/scripts/fubuvalidation.plugin.js",
            };

            ServerHarness.Endpoints.Get<AssetEndpoint>(x => x.get_fubuvalidation())
               .StatusCodeShouldBe(HttpStatusCode.OK)
               .ScriptNames()
               .ShouldHaveTheSameElementsAs(scripts);
        }
    }

    public class AssetEndpoint
    {
        private readonly FubuHtmlDocument _document;

        public AssetEndpoint(FubuHtmlDocument document)
        {
            _document = document;
        }

        public HtmlDocument get_fubuvalidation()
        {
            _document.Asset("fubuvalidation");
            _document.WriteAssetsToHead();

            return _document;
        }
    }
}