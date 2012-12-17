using FubuCore;
using FubuTestingSupport;
using FubuValidation.Tests.Models;
using NUnit.Framework;

namespace FubuValidation.Tests
{
    [TestFixture]
    public class ValidatorTester : InteractionContext<Validator>
    {
        private SimpleModel theModel;
        private RecordingValidationRule theRecordingRule;
        private ValidationContext theContext;
        private ValidationGraph theGraph;

        protected override void beforeEach()
        {
            theModel = new SimpleModel();
            theRecordingRule = new RecordingValidationRule();

            var theSource = ConfiguredValidationSource.For(theModel.GetType(), theRecordingRule);
            theGraph = ValidationGraph.For(theSource);

            Services.Inject<ITypeResolver>(new TypeResolver());
            Services.Inject(theGraph);

            ClassUnderTest.Validate(theModel);

            theContext = theRecordingRule.Context;
        }

        [Test]
        public void sets_the_target_type()
        {
            theContext.TargetType.ShouldEqual(theModel.GetType());
        }

        [Test]
        public void sets_the_type_resolver()
        {
            theContext.Resolver.ShouldEqual(MockFor<ITypeResolver>());
        }

        [Test]
        public void sets_the_service_locator()
        {
            theContext.ServiceLocator.ShouldEqual(MockFor<IServiceLocator>());
        }

        public class RecordingValidationRule : IValidationRule
        {
            public ValidationContext Context;

            public void Validate(ValidationContext context)
            {
                Context = context;
            }
        }
    }
}