using FubuMVC.Core.Registration.Nodes;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class HasInputTypeTester
    {
        private HasInputType theFilter;

        [SetUp]
        public void SetUp()
        {
            theFilter = new HasInputType();
        }

        [Test]
        public void matches_chains_with_input()
        {
            var chain = BehaviorChain.For<HasInputModel>(x => x.Test(null));
            theFilter.Matches(chain).ShouldBeTrue();
        }

        [Test]
        public void does_not_match_chains_without_input()
        {
            var chain = BehaviorChain.For<HasInputModel>(x => x.Test());
            theFilter.Matches(chain).ShouldBeFalse();
        }

        public class HasInputModel
        {
            public HasInputModel Test()
            {
                return this;
            }

            public HasInputModel Test(HasInputModel input)
            {
                return this;
            }
        }
    }
}