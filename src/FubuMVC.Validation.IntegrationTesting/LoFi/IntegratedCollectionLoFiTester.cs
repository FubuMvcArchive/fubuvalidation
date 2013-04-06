using System.Collections.Generic;
using FubuCore;
using FubuMVC.Core.Endpoints;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.IntegrationTesting.LoFi
{
    [TestFixture]
    public class IntegratedCollectionLoFiTester : ValidationHarness
    {
        private LoFiCollectionInput theCollectionInput;

        [SetUp]
        public void SetUp()
        {
            theCollectionInput = new LoFiCollectionInput
                {
                    Collection = new List<LoFiCollectionElement>
                        {
                            new LoFiCollectionElement(),
                            new LoFiCollectionElement(),
                        }
                };
        }

        protected override void configure(Core.FubuRegistry registry)
        {
            registry.Actions.IncludeType<IntegratedLoFiCollectionEdpoint>();
            registry.Import<FubuMvcValidation>();
            registry.Policies.Add(x =>
                {
                    x.Where.InputTypeIs<LoFiCollectionInput>();
                    x.Conneg.ApplyConneg();
                });
        }

        private HttpResponse theCollectionResponse
        {
            get
            {
                // NOTE: PostAsForm does not serialize the Collection member correctly, it is just serializing it by calling to ToString
                // posting the request as Xml does the trick
                return endpoints.PostXml(theCollectionInput);
            }
        }

        [Test]
        public void output_from_collection_endpoint_if_validation_succeeds()
        {
            theCollectionInput.Collection.Each((x, i) => x.Name = "Item{0}".ToFormat(i + 1));
            theCollectionResponse.ReadAsText().ShouldEqual("\"" + IntegratedLoFiCollectionEdpoint.SUCCESS + "\"");
        }

        [Test]
        public void redirects_to_collection_get_if_validation_fails()
        {
            theCollectionInput.Collection.Each(x => x.Name = null);
            theCollectionResponse.ReadAsText().ShouldEqual("\"" + IntegratedLoFiCollectionEdpoint.GET + "\"");
        }
    }
}