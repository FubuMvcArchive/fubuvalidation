using FubuCore;
using FubuMVC.Core.UI.Elements;
using FubuMVC.Core.Urls;
using FubuMVC.Validation.Remote;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class RemoteValidationElementModifierTester
    {
        private RemoteValidationElementModifier theModifier;
        private HtmlTag theTag;
        private ElementRequest theRequest;
        private RemoteRuleGraph theRemoteGraph;
        private InMemoryServiceLocator theServices;
        private IUrlRegistry theUrls;
        private RemoteFieldRule theRemoteRule;

        [SetUp]
        public void SetUp()
        {
            theModifier = new RemoteValidationElementModifier();
            theTag = new HtmlTag("input");

            theRequest = ElementRequest.For<RemoteTarget>(x => x.Username);
            theRequest.ReplaceTag(theTag);

            theRemoteGraph = new RemoteRuleGraph();
            theRemoteRule = theRemoteGraph.RegisterRule(theRequest.Accessor, new UniqueUsernameRule());

            theUrls = new StubUrlRegistry();

            theServices = new InMemoryServiceLocator();
            theServices.Add(theRemoteGraph);
            theServices.Add(theUrls);

            theRequest.Attach(theServices);
        }

        [Test]
        public void always_matches()
        {
            theModifier.Matches(null).ShouldBeTrue();
        }

        [Test]
        public void registers_the_validation_def()
        {
            theModifier.Modify(theRequest);

            theRequest.CurrentTag.Data<RemoteValidationDef>("remote-rule", def =>
            {
                def.url.ShouldEqual(theUrls.RemoteRule());
                def.rules.ShouldHaveTheSameElementsAs(theRemoteRule.ToHash());
            });
        }

        public class RemoteTarget
        {
            public string Username { get; set; }
        }
    }
}