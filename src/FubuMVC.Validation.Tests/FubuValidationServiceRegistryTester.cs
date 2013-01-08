using FubuCore;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class FubuValidationServiceRegistryTester
    {
        private ServiceGraph theGraph;

        [SetUp]
        public void SetUp()
        {
            theGraph = new ServiceGraph();
            new FubuValidationServiceRegistry().As<IServiceRegistration>().Apply(theGraph);
        }

        private ObjectDef verifyDefaultType<TService, TConcrete>()
        {
            var def = theGraph.DefaultServiceFor<TService>();
            def.Type.ShouldEqual(typeof (TConcrete));

            return def;
        }

        [Test]
        public void registers_the_type_resolver()
        {
            verifyDefaultType<ITypeResolver, TypeResolver>();
        }

        [Test]
        public void registers_the_validator()
        {
            verifyDefaultType<IValidator, Validator>();
        }

        [Test]
        public void registers_the_validation_graph_as_singleton()
        {
            theGraph.DefaultServiceFor<ValidationGraph>().IsSingleton.ShouldBeTrue();
        }

        [Test]
        public void registers_the_default_field_rule_registry_as_singleton()
        {
            verifyDefaultType<IFieldRulesRegistry, FieldRulesRegistry>().IsSingleton.ShouldBeTrue();
        }

        [Test]
        public void adds_the_accessor_rules_field_source()
        {
            theGraph.ServicesFor<IFieldValidationSource>().ShouldContain(x => x.Type == typeof(AccessorRulesFieldSource));
        }
    }
}