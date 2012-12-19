using FubuCore;
using FubuMVC.Core.UI.Elements;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using HtmlTags;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class FieldValidationElementModifierTester
    {
        private ElementRequest theRequest;
        private FieldValidationElementModifier theModifier;
        private InMemoryServiceLocator theServices;

        private IFieldValidationModifier theFieldModifier;

        [SetUp]
        public void SetUp()
        {
            theRequest = ElementRequest.For<FieldValidationModifierTarget>(x => x.Name);
            theModifier = new FieldValidationElementModifier();
            theFieldModifier = MockRepository.GenerateStub<IFieldValidationModifier>();

            theRequest.ReplaceTag(new HtmlTag("input"));

            theServices = new InMemoryServiceLocator();
            theServices.Add(ValidationGraph.BasicGraph());
            theServices.Add(theFieldModifier);

            theRequest.Attach(theServices);
        }

        [Test]
        public void always_matches()
        {
            theModifier.Matches(null).ShouldBeTrue();
        }

        [Test]
        public void runs_the_modifier_against_the_field_rules()
        {
            theModifier.Modify(theRequest);
            theFieldModifier.AssertWasCalled(x => x.ModifyFor(new RequiredFieldRule(), theRequest));
        }

        public class FieldValidationModifierTarget
        {
            [Required]
            public string Name { get; set; }
        }
    }
}