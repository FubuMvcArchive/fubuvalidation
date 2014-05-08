using System;
using System.Linq.Expressions;
using System.Net;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Policies;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class ValidationSettingsTester
    {
        private ValidationSettings theSettings;

        [SetUp]
        public void SetUp()
        {
            theSettings = new ValidationSettings();
        }

        private bool matches(Expression<Func<FubuValidationSettingsEndpoint, object>> expression)
        {
            var chain = FubuValidationSettingsGraph.ChainFor(expression);
            return theSettings.As<IApplyValidationFilter>().Filter(chain);
        }

        [Test]
        public void matches_defaults_if_no_filters_are_added()
        {
            matches(x => x.post_the_model(null)).ShouldBeTrue();
        }

        [Test]
        public void matches_specified_filter()
        {
            theSettings.Where.ChainMatches(x => x.InputType() != typeof(SampleAjaxModel));
            matches(x => x.post_json_model(null)).ShouldBeFalse();
        }

        [Test]
        public void defaults_status_code_to_BadRequest()
        {
            theSettings.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Test]
        public void imports_the_modifications()
        {
            theSettings.Import<SampleRegistry>();
            var registry = new FubuRegistry(x =>
            {
                x.Actions.IncludeType<FubuValidationSettingsEndpoint>();
                x.Import<FubuMvcValidation>();
                x.Policies.Local.Add<ValidationPolicy>();
            });

            var graph = BehaviorGraph.BuildFrom(registry);
            var chain = graph.BehaviorFor<FubuValidationSettingsEndpoint>(x => x.post_json_model(null));

            theSettings.As<IChainModification>().Modify(chain);

            chain.ValidationNode().ShouldContain(RenderingStrategies.Inline);
        }

        [Test]
        public void default_excludes_chains_from_activation_that_are_excluded_from_convention()
        {
            theSettings.Where.InputTypeIs<SampleAjaxModel>();

            var graph = BehaviorGraph.BuildFrom(x =>
            {
                x.Actions.IncludeType<FubuValidationSettingsEndpoint>();
                x.Import<FubuMvcValidation>();
            });

            var filter = theSettings.As<IFormActivationFilter>();
            filter.ShouldActivate(graph.BehaviorFor<FubuValidationSettingsEndpoint>(x => x.post_json_model(null))).ShouldBeTrue();
            filter.ShouldActivate(graph.BehaviorFor<FubuValidationSettingsEndpoint>(x => x.post_the_model(null))).ShouldBeFalse();
        }

        [Test]
        public void explicitly_exclude_a_validated_chain_from_form_activation()
        {
            theSettings.Where.InputTypeIs<SampleAjaxModel>();
            theSettings.ExcludeFormActivation.InputTypeIs<SampleAjaxModel>();

            var graph = BehaviorGraph.BuildFrom(x =>
            {
                x.Actions.IncludeType<FubuValidationSettingsEndpoint>();
                x.Import<FubuMvcValidation>();
            });

            var filter = theSettings.As<IFormActivationFilter>();
            filter.ShouldActivate(graph.BehaviorFor<FubuValidationSettingsEndpoint>(x => x.post_json_model(null))).ShouldBeFalse();
            filter.ShouldActivate(graph.BehaviorFor<FubuValidationSettingsEndpoint>(x => x.post_the_model(null))).ShouldBeFalse();
        }

        public class SampleRegistry : ValidationSettingsRegistry
        {
            public SampleRegistry()
            {
                ForInputType<SampleAjaxModel>(x => x.RegisterStrategy(RenderingStrategies.Inline));
            }
        }
    }
}