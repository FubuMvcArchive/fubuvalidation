using System;
using System.Linq.Expressions;
using FubuMVC.Core;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class DefaultValidationFilterTester
    {
        private DefaultValidationChainFilter theFilter;

        [SetUp]
        public void SetUp()
        {
            theFilter = new DefaultValidationChainFilter();
        }

        private bool matches(Expression<Func<FubuValidationSettingsEndpoint, object>> expression)
        {
            var chain = FubuValidationSettingsGraph.ChainFor(expression);
            return theFilter.Matches(chain);
        }

        [Test]
        public void default_includes_lofi_chains_marked_as_http_post()
        {
            matches(x => x.post_the_model(null)).ShouldBeTrue();
        }

        [Test]
        public void default_excludes_lofo_chains_marked_as_http_get()
        {
            matches(x => x.get_the_model(null)).ShouldBeFalse();
        }

        [Test]
        public void default_includes_ajax_chains_marked_as_http_post()
        {
            matches(x => x.post_json_model(null)).ShouldBeTrue();
        }

        [Test]
        public void default_includes_ajax_chains_marked_as_http_get()
        {
            matches(x => x.get_json_model(null)).ShouldBeFalse();
        }
    }

	public class FubuValidationSettingsGraph
    {
        public static BehaviorChain ChainFor(Expression<Func<FubuValidationSettingsEndpoint, object>> expression)
        {
            var registry = new FubuRegistry(x =>
                                            {
                                                x.Actions.IncludeType<FubuValidationSettingsEndpoint>();
                                                x.Import<FubuMvcValidation>();
                                            });

            var graph = BehaviorGraph.BuildFrom(registry);
            return graph.BehaviorFor(expression);
        }
    }

    public class FubuValidationSettingsEndpoint
    {
        public string get_the_model(SampleInputModel input)
        {
            throw new NotImplementedException();
        }

        public string post_the_model(SampleInputModel input)
        {
            throw new NotImplementedException();
        }

        public AjaxContinuation get_json_model(SampleAjaxModel input)
        {
            throw new NotImplementedException();
        }

        public AjaxContinuation post_json_model(SampleAjaxModel input)
        {
            throw new NotImplementedException();
        }
    }
}