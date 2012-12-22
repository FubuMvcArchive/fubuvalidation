using System;
using FubuCore;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.UI;
using FubuMVC.Core.UI.Forms;
using FubuMVC.StructureMap;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class FormValidationSummaryModifierTester : InteractionContext<FormValidationSummaryModifier>
    {
        private BehaviorGraph theGraph;
        private FormRequest theRequest;
        private string theValidationSummary;

        protected override void beforeEach()
        {
            theGraph = BehaviorGraph.BuildFrom(x => x.Actions.IncludeType<ValidationSummaryTargetEndpoint>());
            Services.Inject<IChainResolver>(new ChainResolutionCache(new TypeResolver(), theGraph));


            theRequest = new FormRequest(new ChainSearch { Type = typeof(ValidationSummaryTarget) }, new ValidationSummaryTarget());
            theRequest.Attach(new StructureMapServiceLocator(Services.Container));

            var theForm = new FormTag("test");
            theForm.Append(new HtmlTag("input").Attr("type", "text").Attr("name", "Name"));

            theRequest.ReplaceTag(theForm);

            theValidationSummary = "<div>summary</div>";
            MockFor<IPartialInvoker>().Stub(x => x.Invoke<ValidationSummary>()).Return(theValidationSummary);

            ClassUnderTest.Modify(theRequest);
        }

        [Test]
        public void prepends_the_validation_summary()
        {
            theRequest.CurrentTag.ToString()
                .ShouldEqual("<form method=\"post\" action=\"test\"><div>summary</div><input type=\"text\" name=\"Name\" />");
        }

        public class ValidationSummaryTarget { }

        public class ValidationSummaryTargetEndpoint
        {
            public void post_summary_target(ValidationSummaryTarget target)
            {
                throw new NotImplementedException();
            }
        }
    }
}