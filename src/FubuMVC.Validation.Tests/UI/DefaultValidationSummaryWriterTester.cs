using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Runtime;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using FubuValidation;
using HtmlTags;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class DefaultValidationSummaryWriterTester : InteractionContext<DefaultValidationSummaryWriter>
    {
        [Test]
        public void builds_the_summary_tag()
        {
            ClassUnderTest.BuildSummary().ToString()
                .ShouldEqual("<div class=\"alert alert-error validation-container\" style=\"display:none\"><p>{0}</p><ul class=\"validation-summary\"></ul></div>".ToFormat(ValidationKeys.Summary.ToString()));
        }

        [Test]
        public void writes_the_summary_tag()
        {
            var theTag = new LiteralTag("testing");
            Services.PartialMockTheClassUnderTest();
            ClassUnderTest.Stub(x => x.BuildSummary()).Return(theTag);
            ClassUnderTest.Write(MimeType.Html.Value, Container.GetInstance<FubuRequestContext>(), new ValidationSummary());

            MockFor<IOutputWriter>().AssertWasCalled(x => x.Write(MimeType.Html.ToString(), theTag.ToString()));
        }
    }
}