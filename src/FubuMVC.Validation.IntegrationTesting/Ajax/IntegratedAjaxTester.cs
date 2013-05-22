using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FubuCore;
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
        private AjaxCollectionRequest theCollectionRequest;

        [SetUp]
        public void SetUp()
        {
            theRequest = new AjaxRequest();
            theCollectionRequest = new AjaxCollectionRequest
                {
                    Collection = new List<CollectionItem> {new CollectionItem(), new CollectionItem()}
                };
        }

        protected override void configure(Core.FubuRegistry registry)
        {
            registry.Actions.IncludeType<IntegratedAjaxEndpoint>();
            registry.Import<FubuMvcValidation>();
        }

        private JsonResponse theContinuation
        {
            get
            {
                var response = endpoints.PostJson(theRequest);

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


        private JsonResponse theCollectionContinuation
        {
            get
            {
                var response = endpoints.PostJson(theCollectionRequest);

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
        public void collection_validation_passes()
        {
            theCollectionRequest.Collection.Each((x, i) => x.Name = "Item{0}".ToFormat(i + 1));
            theCollectionContinuation.success.ShouldBeTrue();
        }

        [Test]
        public void validation_error_for_name()
        {
            theRequest.Name = null;
            var errors = theContinuation.errors;

            errors.ShouldHaveCount(1);
            errors.Any(x => x.field == ReflectionHelper.GetAccessor<AjaxRequest>(r => r.Name).Name).ShouldBeTrue();
        }
        [Test]
        public void validation_errors_for_collection_item_names()
        {
            theCollectionRequest.Collection.Each(x => x.Name = null);
            var errors = theCollectionContinuation.errors;

            errors.ShouldHaveCount(2);
            errors.Any(x => x.field == ReflectionHelper.GetAccessor<AjaxCollectionRequest>(r => r.Collection[0].Name).Name).ShouldBeTrue();
            errors.Any(x => x.field == ReflectionHelper.GetAccessor<AjaxCollectionRequest>(r => r.Collection[1].Name).Name).ShouldBeTrue();
        }  
    }

    public class JsonResponse
    {
        public bool success { get; set; }
        public AjaxError[] errors { get; set; }
    }
}