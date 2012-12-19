using Bottles;
using FubuMVC.Core.Registration;
using FubuMVC.Validation.Remote;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class default_validation_services
    {
        private BehaviorGraph theBehaviorGraph;

        [SetUp]
        public void SetUp()
        {
            theBehaviorGraph = BehaviorGraph.BuildFrom(registry => registry.Import<FubuMvcValidation>());
        }

        private void theDefaultServiceIs<TPlugin, TConcrete>()
        {
            theBehaviorGraph
                .Services
                .DefaultServiceFor<TPlugin>()
                .Type
                .ShouldEqual(typeof(TConcrete));
        }

        [Test]
        public void registers_the_default_model_binding_errors()
        {
            theDefaultServiceIs<IModelBindingErrors, ModelBindingErrors>();
        }

        [Test]
        public void registers_the_default_AjaxContinuationResolver()
        {
            theDefaultServiceIs<IAjaxContinuationResolver, AjaxContinuationResolver>();
        }

        [Test]
        public void registers_the_default_AjaxValidationFailureHandler()
        {
            theDefaultServiceIs<IAjaxValidationFailureHandler, AjaxValidationFailureHandler>();
        }

        [Test]
        public void registers_the_default_validation_filter()
        {
            theBehaviorGraph
                .Services
                .DefaultServiceFor(typeof (IValidationFilter<>))
                .Type
                .ShouldEqual(typeof (ValidationFilter<>));
        }

        [Test]
        public void registers_the_remote_rule_graph_as_singleton()
        {
            theBehaviorGraph
                .Services
                .DefaultServiceFor<RemoteRuleGraph>()
                .IsSingleton
                .ShouldBeTrue();
        }

        [Test]
        public void registers_the_default_validation_target_resolver()
        {
            theDefaultServiceIs<IValidationTargetResolver, ValidationTargetResolver>();
        }

        [Test]
        public void registers_the_default_rule_runner()
        {
            theDefaultServiceIs<IRuleRunner, RuleRunner>();
        }

        [Test]
        public void registers_the_remote_rule_graph_activator()
        {
            theBehaviorGraph.Services.ServicesFor<IActivator>().ShouldContain(def => def.Type == typeof(RemoteRuleGraphActivator));
        }

        [Test]
        public void registers_the_default_field_validation_modifier()
        {
            theDefaultServiceIs<IFieldValidationModifier, FieldValidationModifier>();
        }

        [Test]
        public void adds_the_css_validation_annotation_strategy()
        {
            theBehaviorGraph.Services.ServicesFor<IValidationAnnotationStrategy>().ShouldContain(def => def.Type == typeof(CssValidationAnnotationStrategy));
        }
    }
}