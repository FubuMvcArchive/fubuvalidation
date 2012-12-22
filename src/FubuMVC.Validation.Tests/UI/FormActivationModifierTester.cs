using System;
using FubuCore;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.UI.Forms;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class FormActivationModifierTester
    {
        private BehaviorGraph theGraph;
        private IAssetRequirements theRequirements;

        [SetUp]
        public void SetUp()
        {
            theRequirements = MockRepository.GenerateStub<IAssetRequirements>();
            theGraph = BehaviorGraph.BuildFrom(x => x.Actions.IncludeType<FormValidationModeEndpoint>());
        }

        private FormRequest requestFor<T>() where T : class, new()
        {
            var services = new InMemoryServiceLocator();
            services.Add<IChainResolver>(new ChainResolutionCache(new TypeResolver(), theGraph));
            services.Add(theRequirements);

            var request = new FormRequest(new ChainSearch { Type = typeof(T) }, new T());
            request.Attach(services);
            request.ReplaceTag(new FormTag("test"));

            return request;
        }

        [Test]
        public void lofi_mode()
        {
            FormActivationModifier.ModeFor(requestFor<LoFiTarget>()).ShouldEqual(FormActivationModifier.LoFi);
        }

        [Test]
        public void ajax_mode()
        {
            FormActivationModifier.ModeFor(requestFor<AjaxTarget>()).ShouldEqual(FormActivationModifier.Ajax);
        }

        [Test]
        public void writes_the_validationMode_attribute()
        {
            var theRequest = requestFor<AjaxTarget>();
            
            var modifier = new FormActivationModifier();
            modifier.Modify(theRequest);

            theRequest.CurrentTag.ToString()
                .ShouldEqual("<form method=\"post\" action=\"test\" data-validation-mode=\"ajax\" class=\"validated-form\">");
        }

        [Test]
        public void writes_the_validation_activator_requirement()
        {
            var theRequest = requestFor<AjaxTarget>();
            var modifier = new FormActivationModifier();
            modifier.Modify(theRequest);

            theRequirements.AssertWasCalled(x => x.Require("ValidationActivator.js"));
        }

        public class LoFiTarget {}
        public class AjaxTarget {}

        public class FormValidationModeEndpoint
        {
            public LoFiTarget post_lofi(LoFiTarget target)
            {
                throw new NotImplementedException();
            }

            public AjaxContinuation post_ajax(AjaxTarget target)
            {
                throw new NotImplementedException();
            }
        }
    }
}