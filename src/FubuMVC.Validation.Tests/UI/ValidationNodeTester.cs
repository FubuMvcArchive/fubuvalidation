using FubuMVC.Validation.UI;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class ValidationNodeTester
    {
        private ValidationNode theValidationNode;

        [SetUp]
        public void SetUp()
        {
            theValidationNode = new ValidationNode();
        }

        [Test]
        public void no_duplicates()
        {
            var s1 = MockRepository.GenerateStub<IRenderingStrategy>();
            theValidationNode.RegisterStrategy(s1);

            theValidationNode.ShouldHaveTheSameElementsAs(s1);
        }

        [Test]
        public void clears_the_strategies()
        {
            var s1 = MockRepository.GenerateStub<IRenderingStrategy>();
            var s2 = MockRepository.GenerateStub<IRenderingStrategy>();
			
            theValidationNode.RegisterStrategy(s1);
            theValidationNode.RegisterStrategy(s2);

            theValidationNode.Clear();

            theValidationNode.ShouldHaveCount(0);
        }

        [Test]
        public void defaults()
        {
            var strategies = ValidationNode.Default();
            strategies.ShouldHaveTheSameElementsAs(RenderingStrategies.Summary, RenderingStrategies.Highlight);
        }
    }
}