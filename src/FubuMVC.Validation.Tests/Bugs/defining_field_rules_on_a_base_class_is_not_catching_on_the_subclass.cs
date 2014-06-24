using System;
using System.Drawing.Imaging;
using FubuCore;
using FubuCore.Descriptions;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.Bugs
{
    [TestFixture]
    public class defining_field_rules_on_a_base_class_is_not_catching_on_the_subclass
    {
        [Test]
        public void should_be_able_to_resolve_the_field_rules()
        {
            using (var runtime = FubuApplication.DefaultPolicies().StructureMap().Bootstrap())
            {
                var graph = runtime.Factory.Get<ValidationGraph>();

                var plan = graph.PlanFor(typeof (MySubclass));

                plan.FieldRules.RulesFor<MySubclass>(x => x.FirstName).Count().ShouldEqual(2);

                plan.WriteDescriptionToConsole();

                var query = runtime.Factory.Get<FieldValidationQuery>();
                query.RulesFor<MySubclass>(x => x.FirstName).Count().ShouldEqual(2);
            }
        }
    }

    public class MyBase
    {
        public string FirstName { get; set; }
    }

    public class MySubclass : MyBase
    {
        
    }

    public class MySubclassRules : MyBaseRules<MySubclass>
    {
        public MySubclassRules()
        {
            Property(x => x.FirstName).MaximumLength(50);
        }
    }

    public class MyBaseRules<T> : ClassValidationRules<T> where T : MyBase
    {
        public MyBaseRules()
        {
            Require(x => x.FirstName);
        }
    }
}