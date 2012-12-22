using System;
using FubuCore;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.UI.Forms;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class FormValidationModeTester
    {
        private BehaviorGraph theGraph;

        [SetUp]
        public void SetUp()
        {
            theGraph = BehaviorGraph.BuildFrom(x => x.Actions.IncludeType<FormValidationModeEndpoint>());
        }

        private FormRequest requestFor<T>() where T : class, new()
        {
            var services = new InMemoryServiceLocator();
            services.Add<IChainResolver>(new ChainResolutionCache(new TypeResolver(), theGraph));

            var request = new FormRequest(new ChainSearch { Type = typeof(T) }, new T());
            request.Attach(services);
            request.ReplaceTag(new FormTag("test"));

            return request;
        }

        [Test]
        public void lofi_mode()
        {
            FormValidationMode.ModeFor(requestFor<LoFiTarget>()).ShouldEqual(FormValidationMode.LoFi);
        }

        [Test]
        public void ajax_mode()
        {
            FormValidationMode.ModeFor(requestFor<AjaxTarget>()).ShouldEqual(FormValidationMode.Ajax);
        }

        [Test]
        public void writes_the_validationMode_attribute()
        {
            var theRequest = requestFor<AjaxTarget>();
            
            var modifier = new FormValidationMode();
            modifier.Modify(theRequest);

            theRequest.CurrentTag.ToString()
                .ShouldEqual("<form method=\"post\" action=\"test\" data-validation-mode=\"ajax\">");
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