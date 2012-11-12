using System.Diagnostics;
using FubuMVC.Core.Ajax;
using FubuMVC.TestingHarness;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.StructureMap;
using NUnit.Framework;
using StructureMap;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class IntegratedAjaxContinuationFailureTester : FubuRegistryHarness
    {
        protected override void configureContainer(IContainer container)
        {
            container.Configure(x => x.FubuValidation());
        }

        protected override void configure(Core.FubuRegistry registry)
        {
            registry.Actions.IncludeType<AjaxContinuationIntegrationEndpoint>();
            registry.Routes.HomeIs<AjaxContinuationIntegrationEndpoint>(x => x.get_continuation(null));
            registry.Import<FubuValidation>(x => x.Actions.Include(call => true));
        }

        private AjaxContinuation theContinuation
        {
            get
            {
                var response = endpoints.PostJson(new IntegrationAjaxContinuationTarget());
                Debug.WriteLine(response.ReadAsText());

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

        [Test, Ignore("Jeremy to fix by 11/15/2012")]
        public void writes_a_failed_continuation()
        {
            theContinuation.Success.ShouldBeFalse();
        }
    }

    public class AjaxContinuationIntegrationEndpoint
    {
        public AjaxContinuation get_continuation(IntegrationAjaxContinuationTarget target)
        {
            return AjaxContinuation.Successful();
        }
    }

    public class IntegrationAjaxContinuationTarget
    {
        [Required]
        public string Message { get; set; }
    }
}