using System;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Http;
using FubuMVC.Core.Http.Owin;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.UI.Forms;
using FubuMVC.Core.Urls;
using FubuMVC.Validation.UI;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests.UI
{
    [TestFixture]
    public class FormValidationModifierTester
    {
        private BehaviorGraph theGraph;
        private IAssetTagBuilder theRequirements;
        private ValidationSettings theSettings;

        [SetUp]
        public void SetUp()
        {
            theSettings = new ValidationSettings();
            theRequirements = MockRepository.GenerateStub<IAssetTagBuilder>();
            theGraph = BehaviorGraph.BuildFrom(x =>
            {
                x.Actions.IncludeType<FormValidationModeEndpoint>();
                x.Import<FubuMvcValidation>();
            });
        }

        private FormRequest requestFor<T>() where T : class, new()
        {
            var services = new InMemoryServiceLocator();
            services.Add<IChainResolver>(new ChainResolutionCache(new TypeResolver(), theGraph));
            services.Add(theRequirements);
            services.Add<IChainUrlResolver>(new ChainUrlResolver(new OwinHttpRequest()));
            services.Add<ITypeResolver>(new TypeResolver());
            services.Add(new AccessorRules());
            services.Add<ITypeDescriptorCache>(new TypeDescriptorCache());
            services.Add(theSettings);

            var request = new FormRequest(new ChainSearch { Type = typeof(T) }, new T());
            request.Attach(services);
            request.ReplaceTag(new FormTag("test"));

            return request;
        }

        [Test]
        public void modifies_the_form()
        {
            var theRequest = requestFor<AjaxTarget>();

            var modifier = new FormValidationModifier();
            modifier.Modify(theRequest);

            theRequest.CurrentTag.Data("validation-summary").ShouldEqual(true);
            theRequest.CurrentTag.Data("validation-highlight").ShouldEqual(true);
            theRequest.CurrentTag.HasClass("validated-form").ShouldBeTrue();
        }

        [Test]
        public void excluded_from_activation()
        {
            theSettings.ExcludeFormActivation.InputTypeIs<AjaxTarget>();
            var theRequest = requestFor<AjaxTarget>();

            var modifier = new FormValidationModifier();
            modifier.Modify(theRequest);

            theRequest.CurrentTag.Data("validation-summary").ShouldEqual(true);
            theRequest.CurrentTag.Data("validation-highlight").ShouldEqual(true);
            theRequest.CurrentTag.HasClass("validated-form").ShouldBeTrue();

            theRequirements.AssertWasNotCalled(x => x.RequireScript("ValidationActivator.js"));
        }

        [Test]
        public void no_strategies()
        {
            var theRequest = requestFor<AjaxTarget>();
            theRequest.Chain.ValidationNode().Clear();

            var modifier = new FormValidationModifier();
            modifier.Modify(theRequest);

            theRequest.CurrentTag.HasClass("validated-form").ShouldBeFalse();
        }

        [Test]
        public void adds_the_validation_options()
        {
            var theRequest = requestFor<AjaxTarget>();
            var modifier = new FormValidationModifier();
            modifier.Modify(theRequest);

            var options = ValidationOptions.For(theRequest);

            theRequest.CurrentTag.Data(ValidationOptions.Data).ShouldEqual(options);
        }

        [Test]
        public void writes_the_validation_activator_requirement()
        {
            var theRequest = requestFor<AjaxTarget>();
            var modifier = new FormValidationModifier();
            modifier.Modify(theRequest);

            theRequirements.AssertWasCalled(x => x.RequireScript("ValidationActivator.js"));
        }
    }

    public class LoFiTarget
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    }

    public class AjaxTarget { }
    [NotValidated]
    public class NoneTarget { }
    public class IgnoredTarget { }

    public class FormValidationModeEndpoint
    {
        public NoneTarget post_none(NoneTarget target)
        {
            throw new NotImplementedException();
        }

        [NotValidated]
        public IgnoredTarget post_ignored(IgnoredTarget target)
        {
            throw new NotImplementedException();
        }

        public LoFiTarget post_lofi(LoFiTarget target)
        {
            throw new NotImplementedException();
        }

        public AjaxContinuation post_ajax(AjaxTarget target)
        {
            throw new NotImplementedException();
        }
    }
}