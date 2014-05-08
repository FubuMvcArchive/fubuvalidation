using System;
using System.Linq;
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

        private const string theValidationSummary = "<div>summary</div>";

        protected override void beforeEach()
        {
            theGraph = BehaviorGraph.BuildFrom(x => x.Actions.IncludeType<ValidationSummaryTargetEndpoint>());
            Services.Inject<IChainResolver>(new ChainResolutionCache(new TypeResolver(), theGraph));


            theRequest = new FormRequest(new ChainSearch {Type = typeof (ValidationSummaryTarget)},
                new ValidationSummaryTarget());
            theRequest.Attach(new StructureMapServiceLocator(Services.Container));

            ValidationPolicy.ApplyValidation(theRequest.Chain.FirstCall(), new ValidationSettings());
            theRequest.Chain.ValidationNode().Clear();
            theRequest.Chain.ValidationNode().RegisterStrategy(RenderingStrategies.Summary);

            var theForm = new FormTag("test");
            theForm.Append(new HtmlTag("input").Attr("type", "text").Attr("name", "Name"));

            theRequest.ReplaceTag(theForm);

            MockFor<IPartialInvoker>().Stub(x => x.Invoke<ValidationSummary>()).Return(theValidationSummary);
        }

        [Test]
        public void no_summary_if_the_summary_strategy_is_not_registered()
        {
            theRequest.Chain.ValidationNode().Clear();
            ClassUnderTest.Modify(theRequest);
            theRequest.CurrentTag.Children.Count.ShouldEqual(1);
            theRequest.CurrentTag.Children[0].TagName().ShouldEqual("input");
        }

        [Test]
        public void prepends_the_validation_summary()
        {
            ClassUnderTest.Modify(theRequest);
            theRequest.CurrentTag.Children.Count.ShouldEqual(2);

            theRequest.CurrentTag.Children[0].ToString().ShouldEqual(theValidationSummary);

            theRequest.CurrentTag.Children[1].TagName().ShouldEqual("input");
        }

        public class ValidationSummaryTarget
        {
        }

        public class ValidationSummaryTargetEndpoint
        {
            public void post_summary_target(ValidationSummaryTarget target)
            {
                throw new NotImplementedException();
            }
        }
    }
}