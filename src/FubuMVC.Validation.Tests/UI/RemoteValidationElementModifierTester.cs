using System;
using System.Collections.Generic;
using FubuCore;
using FubuCore.Reflection;
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
        private HtmlTag theTag, theNestedTag, theItemTag;
        private ElementRequest theRequest, theNestedRequest, theItemRequest;
        private RemoteRuleGraph theRemoteGraph;
        private InMemoryServiceLocator theServices;
        private IUrlRegistry theUrls;
        private RemoteFieldRule theRemoteRule, theNestedRemoteRule, theItemRemoteRule;

        [SetUp]
        public void SetUp()
        {
            theModifier = new RemoteValidationElementModifier();
            theTag = new HtmlTag("input");
            theNestedTag = new HtmlTag("input");
            theItemTag = new HtmlTag("input");

            theRequest = ElementRequest.For<RemoteTarget>(x => x.Username);
            theRequest.ReplaceTag(theTag);

            theNestedRequest = ElementRequest.For<NestedRemoteTarget>(x => x.NestedProperty.StringProperty);
            theNestedRequest.ReplaceTag(theNestedTag);

            theItemRequest = ElementRequest.For<NestedRemoteTarget>(x => x.NestedProperty.Items[3].IntProperty);
            theItemRequest.ReplaceTag(theItemTag);

            theRemoteGraph = new RemoteRuleGraph();
            theRemoteRule = theRemoteGraph.RegisterRule(theRequest.Accessor, new UniqueUsernameRule());
            theNestedRemoteRule = theRemoteGraph.RegisterRule(ReflectionHelper.GetAccessor<Nested>(p => p.StringProperty), new RemoteStringRule());
            theItemRemoteRule = theRemoteGraph.RegisterRule(ReflectionHelper.GetAccessor<Item>(p => p.IntProperty), new RemoteIntRule());

            theUrls = new StubUrlRegistry();

            theServices = new InMemoryServiceLocator();
            theServices.Add(theRemoteGraph);
            theServices.Add(theUrls);

            new[] {theRequest, theNestedRequest, theItemRequest}.Each(x => x.Attach(theServices));
        }

        [Test]
        public void always_matches()
        {
            theModifier.Matches(null).ShouldBeTrue();
        }

        [Test]
        public void registers_the_validation_def()
        {
            new[]
                {
                    Tuple.Create(theRequest, theRemoteRule),
                    Tuple.Create(theNestedRequest, theNestedRemoteRule),
                    Tuple.Create(theItemRequest, theItemRemoteRule)
                }
                .Each(x =>
                    {
                        theModifier.Modify(x.Item1);
                        var def = x.Item1.CurrentTag.Data("remote-rule").As<RemoteValidationDef>();
                        def.url.ShouldEqual(theUrls.RemoteRule());
                        def.rules.ShouldHaveTheSameElementsAs(x.Item2.ToHash());
                    });

        }
        
        [Test]
        public void no_registration_when_no_rules_are_found()
        {
            theRemoteGraph = new RemoteRuleGraph();
            theServices.Add(theRemoteGraph);

            new[]
                {
                    theRequest,
                    theNestedRequest,
                    theItemRequest
                }
                .Each(x =>
                    {
                        theModifier.Modify(x);
                        x.CurrentTag.Data("remote-rule").ShouldBeNull();
                    });

        }

        public class RemoteTarget
        {
            public string Username { get; set; }
        }

        public class NestedRemoteTarget
        {
            public Nested NestedProperty { get; set; }
        }

        public class Nested
        {
            public List<Item> Items { get; set; }
            public string StringProperty { get; set; }
        }

        public class Item
        {
            public int IntProperty { get; set; }
        }
    }
}