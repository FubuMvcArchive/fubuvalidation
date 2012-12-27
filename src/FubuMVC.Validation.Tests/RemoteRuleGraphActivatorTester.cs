using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bottles;
using Bottles.Diagnostics;
using FubuCore.Reflection;
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
            var rule = theRuleGraph.RulesFor(ReflectionHelper.GetAccessor<ActivatorTargetWithRemotes>(x => x.Username)).Single();
            rule.Type.ShouldEqual(typeof(UniqueUsernameRule));

            theRuleGraph.RulesFor(ReflectionHelper.GetAccessor<ActivatorTargetWithRemotes>(x => x.Name)).ShouldHaveCount(0);
            theRuleGraph.RulesFor(ReflectionHelper.GetAccessor<ActivatorTargetNoRemotes>(x => x.Name)).ShouldHaveCount(0);
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


    }

    public class UniqueUsernameRule : IRemoteFieldValidationRule
    {
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
}