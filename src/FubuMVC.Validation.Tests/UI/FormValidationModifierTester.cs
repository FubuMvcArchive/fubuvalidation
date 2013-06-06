using System;
using FubuCore;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.UI.Forms;
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
        private IAssetRequirements theRequirements;

        [SetUp]
        public void SetUp()
        {
            theRequirements = MockRepository.GenerateStub<IAssetRequirements>();
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
            services.Add<ICurrentHttpRequest>(new StandInCurrentHttpRequest());

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

            theRequest.CurrentTag.ToString()
				.ShouldEqual("<form method=\"post\" action=\"test\" data-validation-summary=\"true\" data-validation-highlight=\"true\" class=\"validated-form\">");
        }

		[Test]
		public void no_strategies()
		{
			var theRequest = requestFor<AjaxTarget>();
			theRequest.Chain.ValidationNode().Clear();

			var modifier = new FormValidationModifier();
			modifier.Modify(theRequest);

			theRequest.CurrentTag.ToString()
				.ShouldEqual("<form method=\"post\" action=\"test\">");
		}

        [Test]
        public void writes_the_validation_activator_requirement()
        {
            var theRequest = requestFor<AjaxTarget>();
            var modifier = new FormValidationModifier();
            modifier.Modify(theRequest);

            theRequirements.AssertWasCalled(x => x.Require("ValidationActivator.js"));
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
	public class IgnoredTarget {}

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