using System;
using System.Linq.Expressions;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Policies;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class CompositeChainFilterTester
    {
        private CompositeChainFilter theFilter;

        [SetUp]
        public void SetUp()
        {
            theFilter = new CompositeChainFilter(new HttpMethodFilter("GET"), new InputTypeIs<int>());
        }

        [Test]
        public void matches_chains_that_match_all_criteria()
        {
            matches(x => x.get_something(null)).ShouldBeFalse();
            matches(x => x.post_something(null)).ShouldBeFalse();
            matches(x => x.get_something_else(0)).ShouldBeTrue();
        }

        private BehaviorChain chainFor(Expression<Func<CompositeChainFilterEndpoint, object>> expression)
        {
            var registry = new FubuRegistry(x =>
                                            {
                                                x.Actions.IncludeType<CompositeChainFilterEndpoint>();
                                                x.Import<FubuMvcValidation>();
                                            });

            var graph = BehaviorGraph.BuildFrom(registry);
            return graph.BehaviorFor(expression);
        }

        private bool matches(Expression<Func<CompositeChainFilterEndpoint, object>> expression)
        {
            var chain = chainFor(expression);
            return theFilter.Matches(chain);
        }

        public class CompositeChainFilterEndpoint
        {
            public string get_something(string input)
            {
                throw new NotImplementedException();
            }

            public string get_something_else(int input)
            {
                throw new NotImplementedException();
            }

            public string post_something(string input)
            {
                throw new NotImplementedException();
            }
        }
    }
}