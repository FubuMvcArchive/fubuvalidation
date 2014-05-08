using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.UI.Elements;
using FubuMVC.StructureMap;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using NUnit.Framework;
using StructureMap;

namespace FubuMVC.Validation.IntegrationTesting.Bugs
{
    [TestFixture]
    public class applicability_of_annotation_strategies_with_shared_interface_validation_rules
    {
        [Test]
        public void fix_it()
        {
            var container = new Container(x => {
                x.For<IValidationAnnotationStrategy>().Add<RequiredAccessibilityAttributesStrategy>();

            });

            var registry = new FubuRegistry();
            registry.Policies.Local.Add<ValidationPolicy>();

            using (var runtime = FubuApplication.For(registry).StructureMap(container).Bootstrap())
            {
                var validationGraph = container.GetInstance<ValidationGraph>();
                var query = validationGraph.Query();
                var rules = query.RulesFor<BasicOne>(x => x.Name);
                rules.Single().ShouldBeOfType<RequiredFieldRule>();
            }
        }
    }

    public class RequiredAccessibilityAttributesStrategy : IValidationAnnotationStrategy
    {
        private static readonly List<Type> Rules = new List<Type>();

        static RequiredAccessibilityAttributesStrategy()
        {
            DefineClass<RequiredFieldRule>();
        }

        public bool Matches(IFieldValidationRule rule)
        {
            return Rules.Contains(rule.GetType());
        }

        private static void DefineClass<T>()
        {
            Rules.Add(typeof(T));
        }

        public void Modify(ElementRequest request, IFieldValidationRule rule)
        {
            request.CurrentTag.Attr("required");
            request.CurrentTag.Attr("aria-required", true);
        }
    }

    public interface IBasicInformation
    {
        string Name { get; set; }
    }

    public class BasicOne : IBasicInformation
    {
        public string Name { get; set; }
    }

    public class BasicTwo : IBasicInformation
    {
        public string Name { get; set; }
    }

    public class BasicRules<T> : ClassValidationRules<T> where T : class, IBasicInformation
    {
        public BasicRules()
        {
            Property(x => x.Name).Required();
        }
    }

    public class BasicOneValidation : BasicRules<BasicOne>
    {
        
    }

    public class BasicTwoValidation : BasicRules<BasicTwo>
    {

    }
}