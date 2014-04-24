using Bottles;
using Bottles.Diagnostics;
using FubuCore.Reflection;
using FubuMVC.Core.Registration;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class ValidationRegistrationActivatorTester : InteractionContext<ValidationRegistrationActivator>
    {
        private ValidationGraph theGraph;
        private TypePool theTypes;

        protected override void beforeEach()
        {
            theGraph = ValidationGraph.BasicGraph();
            Services.Inject(theGraph);

            theTypes = new TypePool();
            theTypes.AddAssembly(typeof(RegistrationTargetRules).Assembly);

            Services.PartialMockTheClassUnderTest();
            ClassUnderTest.Stub(x => x.Types()).Return(theTypes);

            ClassUnderTest.Activate(new IPackageInfo[0], new PackageLog());
        }

        [Test]
        public void runs_the_registration()
        {
            var query = theGraph.Query();
            query.HasRule<RequiredFieldRule>(ReflectionHelper.GetAccessor<RegistrationTarget>(x => x.FirstName)).ShouldBeTrue();
            query.HasRule<EmailFieldRule>(ReflectionHelper.GetAccessor<RegistrationTarget>(x => x.Email)).ShouldBeTrue();
        }

        [Test]
        public void matches_concrete_types_of_validation_registration_with_default_ctor()
        {
            ValidationRegistrationActivator.IsValidationRegistration(typeof(RegistrationTargetRules)).ShouldBeTrue();
        }

        [Test]
        public void no_match_for_concrete_type_with_no_default_ctor()
        {
            ValidationRegistrationActivator.IsValidationRegistration(typeof(BadRules)).ShouldBeFalse();
        }

        [Test]
        public void no_match_for_open_generics()
        {
            ValidationRegistrationActivator.IsValidationRegistration(typeof(ClassValidationRules<>)).ShouldBeFalse();
        }

        [Test]
        public void no_match_for_non_validation_registration_types()
        {
            ValidationRegistrationActivator.IsValidationRegistration(typeof(FieldValidationQuery)).ShouldBeFalse();
        }

        public class BadRules : ClassValidationRules<RegistrationTarget>
        {
            public BadRules(string test) { }
        }

        public class RegistrationTargetRules : ClassValidationRules<RegistrationTarget>
        {
            public RegistrationTargetRules()
            {
                Property(x => x.FirstName).Required();
                Property(x => x.Email).Email();
            }
        }

        public class RegistrationTarget
        {
            public string FirstName { get; set; }
            public string Email { get; set; }
        }
    }
}