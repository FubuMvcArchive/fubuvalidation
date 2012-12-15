using System;
using FubuCore;
using FubuTestingSupport;
using FubuValidation.Tests.Models;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuValidation.Tests
{
    [TestFixture]
    public class ValidatorTester : InteractionContext<Validator>
    {
        private Type theType;
        private SimpleModel theModel;
        private RecordingValidationRule theRecordingRule;
        private ValidationContext theContext;

        protected override void beforeEach()
        {
            theModel = new SimpleModel();
            theType = typeof (ContactModel);
            theRecordingRule = new RecordingValidationRule();

            MockFor<ITypeResolver>()
                .Stub(x => x.ResolveType(theModel))
                .Return(theType);

            MockFor<IValidationQuery>()
                .Stub(x => x.RulesFor(theType))
                .Return(new IValidationRule[] {theRecordingRule});

            ClassUnderTest.Validate(theModel);

            theContext = theRecordingRule.Context;
        }

        [Test]
        public void sets_the_target_type()
        {
            theContext.TargetType.ShouldEqual(theType);
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