using FubuCore;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class DefaultValidationSummaryNodeTester
    {
        private DefaultValidationSummaryNode theNode;

        [SetUp]
        public void SetUp()
        {
            theNode = new DefaultValidationSummaryNode();
        }

        [Test]
        public void applies_to_login_request()
        {
            theNode.ResourceType.ShouldEqual(typeof(ValidationSummary));
        }

        [Test]
        public void applies_to_html()
        {
            theNode.Mimetypes.ShouldHaveTheSameElementsAs(MimeType.Html.Value);
        }

        [Test]
        public void builds_the_default_login_request_writer()
        {
            var def = theNode.As<IContainerModel>().ToObjectDef();
            def.FindDependencyDefinitionFor(typeof(IMediaWriter<ValidationSummary>)).Type.ShouldEqual(
                typeof(DefaultValidationSummaryWriter));
        }
    }
}