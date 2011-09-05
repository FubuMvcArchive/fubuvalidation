using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;
using Fluent = FluentValidation;

namespace FubuValidation.FluentValidation.Tests
{
    [TestFixture]
    public class FluentValidationSourceTester : InteractionContext<FluentValidationSource>
    {
        [Test]
        public void should_yield_no_rules_if_no_validators_match()
        {
            var modelType = typeof (SampleFluentModel);
            MockFor<Fluent.IValidator>()
                .Expect(v => v.CanValidateInstancesOfType(modelType))
                .Return(false);

            ClassUnderTest
                .RulesFor(modelType)
                .ShouldHaveCount(0);
        }

        [Test]
        public void should_return_configured_rule_for_matching_validators()
        {
            var validator = new SampleValidator();
            Container
                .Configure(x => x.For<Fluent.IValidator>().Add(validator));

            var rule = ClassUnderTest
                .RulesFor(typeof (SampleFluentModel))
                .OfType<FluentValidationRule>()
                .First();

            rule
                .Validators
                .ShouldContain(validator);
        }
        
    }
}