using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Bottles;
using Bottles.Diagnostics;
using FubuCore.Reflection;
using FubuLocalization;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Registration;
using FubuMVC.Validation.Remote;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class RemoteRuleGraphActivatorTester
    {
        private ValidationGraph theValidationGraph;
        private RemoteRuleGraph theRuleGraph;
        private BehaviorGraph theBehaviorGraph;
        private RemoteRuleQuery theQuery;
        private RemoteRuleGraphActivator theActivator;

        [SetUp]
        public void SetUp()
        {
            theBehaviorGraph = BehaviorGraph.BuildFrom(r =>
            {
                r.Actions.IncludeType<RemoteRuleGraphEndpoint>();
                r.Import<FubuMvcValidation>();
            });

            theValidationGraph = ValidationGraph.BasicGraph();
            theRuleGraph = new RemoteRuleGraph();
            theQuery = RemoteRuleQuery.Basic();

            theActivator = new RemoteRuleGraphActivator(theValidationGraph, theRuleGraph, theBehaviorGraph, theQuery, new TypeDescriptorCache());

            theActivator.Activate(new IPackageInfo[0], new PackageLog());
        }

        [Test]
        public void fills_the_rules_in_the_rule_graph()
        {
            var rule = remotesFor<ActivatorTargetWithRemotes>(x => x.Username).Single();
            rule.Type.ShouldEqual(typeof(UniqueUsernameRule));

            remotesFor<ActivatorTargetWithRemotes>(x => x.Name).ShouldHaveCount(0);
            remotesFor<ActivatorTargetNoRemotes>(x => x.Name).ShouldHaveCount(0);
        }

        [Test]
        public void register_remote_rules_for_nested_properties()
        {
            remotesFor<ActivatorTargetWithNestedRemotes>(x => x.ModelNestedProperty.NestedDateProperty)
                .ShouldHaveCount(1).All(x => x.Rule is RemoteDateRule).ShouldBeTrue();
            remotesFor<ActivatorTargetWithNestedRemotes>(x => x.ModelNestedProperty.NestedIntProperty)
                .ShouldHaveCount(1).All(x => x.Rule is RemoteIntRule).ShouldBeTrue();
            remotesFor<ActivatorTargetWithNestedRemotes>(x => x.ModelNestedProperty.NestedStringProperty)
                .ShouldHaveCount(1).All(x => x.Rule is RemoteStringRule).ShouldBeTrue();
        }

        [Test]
        public void register_remote_rules_for_collection_closing_types()
        {
            remotesFor<ActivatorTargetWithNestedRemotes>(x => x.ModelNestedProperty.Items[0].ItemDateProperty)
                .ShouldHaveCount(1).All(x => x.Rule is RemoteDateRule).ShouldBeTrue();
            remotesFor<ActivatorTargetWithNestedRemotes>(x => x.ModelNestedProperty.Items[1].ItemIntProperty)
                .ShouldHaveCount(1).All(x => x.Rule is RemoteIntRule).ShouldBeTrue();
            remotesFor<ActivatorTargetWithNestedRemotes>(x => x.ModelNestedProperty.Items[2].ItemStringProperty)
                .ShouldHaveCount(1).All(x => x.Rule is RemoteStringRule).ShouldBeTrue();
        }

        private IEnumerable<RemoteFieldRule> remotesFor<T>(Expression<Func<T, object>> expression)
        {
            return theRuleGraph.RulesFor(expression.ToAccessor());
        }

        public class RemoteRuleGraphEndpoint
        {
            public AjaxContinuation get_remotes(ActivatorTargetWithRemotes request)
            {
                throw new NotImplementedException();
            }

            public AjaxContinuation get_no_remotes(ActivatorTargetNoRemotes request)
            {
                throw new NotImplementedException();
            }
            public AjaxContinuation get_nested_remotes(ActivatorTargetWithNestedRemotes input)
            {
                throw new NotImplementedException();
            }
        }

        public class ActivatorTargetWithRemotes
        {
            [UniqueUsername]
            public string Username { get; set; }

            public string Name { get; set; }
        }

        public class ActivatorTargetNoRemotes
        {
            [Required]
            public string Name { get; set; }
        }

        public class ActivatorTargetWithNestedRemotes
        {
            public string ModelStringProperty { get; set; }
            public int ModelIntProperty { get; set; }
            public DateTime ModelDateProperty { get; set; }
            public Nested ModelNestedProperty { get; set; }
        }

        public class Nested
        {
            public List<Item> Items { get; set; }
            [RemoteStringRule]
            [MaximumStringLength(10)]
            public string NestedStringProperty { get; set; }
            [RemoteIntRule]
            public int NestedIntProperty { get; set; }
            [RemoteDateRule]
            public DateTime NestedDateProperty { get; set; }
            public Nested NestedNestedProperty { get; set; }
        }

        public class Item
        {
            [RemoteStringRule]
            public string ItemStringProperty { get; set; }
            [RemoteIntRule]
            public int ItemIntProperty { get; set; }
            [RemoteDateRule]
            public DateTime ItemDateProperty { get; set; }
        }

    }

    public class UniqueUsernameRule : IRemoteFieldValidationRule
    {
        public StringToken Token { get; set; }

        public void Validate(Accessor accessor, ValidationContext context)
        {
            throw new System.NotImplementedException();
        }
    }

    public class UniqueUsernameAttribute : FieldValidationAttribute
    {
        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new UniqueUsernameRule();
        }
    }


    public class RemoteStringRuleAttribute : FieldValidationAttribute
    {
        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new RemoteStringRule();
        }
    }


    public class RemoteStringRule : IRemoteFieldValidationRule
    {
        public void Validate(Accessor accessor, ValidationContext context)
        {
        }

        public StringToken Token { get; set; }
    }

    public class RemoteIntRule : IRemoteFieldValidationRule
    {
        public void Validate(Accessor accessor, ValidationContext context)
        {
        }

        public StringToken Token { get; set; }
    }
    public class RemoteIntRuleAttribute : FieldValidationAttribute
    {
        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new RemoteIntRule();
        }
    }

    public class RemoteDateRule : IRemoteFieldValidationRule
    {
        public void Validate(Accessor accessor, ValidationContext context)
        {
        }

        public StringToken Token { get; set; }
    }
    public class RemoteDateRuleAttribute : FieldValidationAttribute
    {
        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new RemoteDateRule();
        }
    }
}