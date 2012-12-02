using FubuMVC.Core.Registration;
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
    }
}