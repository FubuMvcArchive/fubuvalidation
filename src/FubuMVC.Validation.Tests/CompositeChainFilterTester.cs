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
            theFilter = new CompositeChainFilter(new HttpMethodFilter("GET"), new InputTypeIs<Random>());
        }

        [Test]
        public void matches_chains_that_match_all_criteria()
        {
            matches(x => x.get_something(null)).ShouldBeFalse();
            matches(x => x.post_something(null)).ShouldBeFalse();
            matches(x => x.get_something_else(null)).ShouldBeTrue();
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
            public string get_something(Input1 input)
            {
                throw new NotImplementedException();
            }

            public string get_something_else(Random input)
            {
                throw new NotImplementedException();
            }

            public string post_something(Input3 input)
            {
                throw new NotImplementedException();
            }
        }

        public class Random{}
        public class Input1{}
        public class Input2{}
        public class Input3{}
    }
}