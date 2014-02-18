using FubuCore;
using FubuLocalization;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.UI.Forms;
using FubuMVC.Core.Urls;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using FubuValidation;
using HtmlTags;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class NotificationSerializationModifierTester
    {
        private BehaviorGraph theGraph;
        private IAssetRequirements theRequirements;
        private NotificationSerializationModifier theModifier;
        private Notification theNotification;
        private IFubuRequest theRequest;

        private AjaxContinuation theContinuation;

        [SetUp]
        public void SetUp()
        {
            theRequirements = MockRepository.GenerateStub<IAssetRequirements>();
            theGraph = BehaviorGraph.BuildFrom(x =>
            {
                x.Actions.IncludeType<FormValidationModeEndpoint>();
                x.Import<FubuMvcValidation>();
            });

            theModifier = new NotificationSerializationModifier();
        }

        private FormRequest requestFor<T>() where T : class, new()
        {
            var services = new InMemoryServiceLocator();
            services.Add<IChainResolver>(new ChainResolutionCache(new TypeResolver(), theGraph));
            services.Add(theRequirements);
            services.Add<IChainUrlResolver>(new ChainUrlResolver(new StandInCurrentHttpRequest()));

            theRequest = new InMemoryFubuRequest();
            theNotification = Notification.Valid();
            theRequest.Set(theNotification);

            services.Add(theRequest);

            var request = new FormRequest(new ChainSearch {Type = typeof (T)}, new T());
            request.Attach(services);
            request.ReplaceTag(new FormTag("test"));

            theContinuation = AjaxContinuation.Successful();
            theContinuation.ShouldRefresh = true;

            var resolver = MockRepository.GenerateStub<IAjaxContinuationResolver>();
            resolver.Stub(x => x.Resolve(theNotification)).Return(theContinuation);

            services.Add(resolver);

            return request;
        }

        [Test]
        public void does_nothing_if_the_notification_is_valid()
        {
            var request = requestFor<LoFiTarget>();
            theModifier.Modify(request);

            request.CurrentTag.ToString().ShouldEqual("<form method=\"post\" action=\"test\">");
        }

        [Test]
        public void serializes_the_continuation_if_the_notification_is_invalid()
        {
            var request = requestFor<LoFiTarget>();
            theNotification.RegisterMessage(StringToken.FromKeyString("Test", "Test"));
            theModifier.Modify(request);

            request.CurrentTag.Data("validation-results").ShouldEqual(theContinuation.ToDictionary());
        }

        [Test]
        public void writes_the_validation_results_activator_requirement()
        {
            var request = requestFor<LoFiTarget>();
            theNotification.RegisterMessage(StringToken.FromKeyString("Test", "Test"));
            theModifier.Modify(request);

            theRequirements.AssertWasCalled(x => x.Require("ValidationResultsActivator.js"));
        }
    }
}