using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class ValidationBehaviorChainExtensionsTester
    {
        [Test]
        public void finds_the_validation_node()
        {
            var stub = new StubValidationBehaviorNode
            {
                Validation = ValidationNode.Default()
            };

            var chain = new BehaviorChain();
            chain.AddToEnd(stub);

            chain.ValidationNode().ShouldEqual(stub.Validation);
        }

        [Test]
        public void returns_empty_if_no_validation_node_exists()
        {
            var chain = new BehaviorChain();
            chain.ValidationNode().ShouldEqual(ValidationNode.Empty());
        }

        public class StubValidationBehaviorNode : BehaviorNode, IHaveValidation
        {
            protected override ObjectDef buildObjectDef()
            {
                throw new System.NotImplementedException();
            }

            public override BehaviorCategory Category
            {
                get { throw new System.NotImplementedException(); }
            }

            public ValidationNode Validation { get; set; }
        }
    }
}