using FubuTestingSupport;
using FubuValidation.Fields;
using NUnit.Framework;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace FubuValidation.StructureMap.Tests
{
    [TestFixture]
    public class ValidationBootstrapExtensionsTester
    {
        private IContainer _container;

        [SetUp]
        public void before_each()
        {
            _container = new Container(x => x.AddRegistry<ValidationTestRegistry>());
        }

        [Test]
        public void should_register_default_validator()
        {
            _container
                .GetInstance<IValidator>()
                .ShouldNotBeNull();
        }

        [Test]
        public void should_register_field_rule_source()
        {
            _container
                .Model
                .InstancesOf<IValidationSource>()
                .ShouldContain(r => r.ConcreteType== typeof(FieldRuleSource));
        }

        [Test]
        public void should_register_field_rule_registry()
        {
            _container
                .GetInstance<IFieldRulesRegistry>()
                .ShouldNotBeNull();
        }

        [Test]
        public void should_register_validation_query()
        {
            _container
                .GetInstance<IValidationQuery>()
                .ShouldNotBeNull();
        }

        [Test]
        public void should_register_field_validation_query()
        {
            _container
                .GetInstance<IFieldValidationQuery>()
                .ShouldNotBeNull();
        }

        public class ValidationTestRegistry : Registry
        {
            public ValidationTestRegistry()
            {
                this.FubuValidation();
            }
        }
    }
}