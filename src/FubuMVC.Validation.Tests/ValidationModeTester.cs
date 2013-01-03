using System;
using FubuCore;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.UI.Forms;
using FubuMVC.Validation.Tests.UI;
using FubuTestingSupport;
using HtmlTags;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
	[TestFixture]
	public class ValidationModeTester
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
		public void ajax_mode()
		{
			ValidationMode.For(requestFor<AjaxTarget>()).ShouldEqual(ValidationMode.Ajax);
		}

		[Test]
		public void lofi_mode()
		{
			ValidationMode.For(requestFor<LoFiTarget>()).ShouldEqual(ValidationMode.LoFi);
		}

		[Test]
		public void none()
		{
			ValidationMode.For(requestFor<NoneTarget>()).ShouldEqual(ValidationMode.None);
		}

		[Test]
		public void no_modification_to_tag()
		{
			formForMode(ValidationMode.None)
				.ToString().ShouldEqual("<form method=\"post\" action=\"test\">");
		}

		[Test]
		public void modify_the_tag_for_ajax()
		{
			formForMode(ValidationMode.Ajax)
				.ToString().ShouldEqual("<form method=\"post\" action=\"test\" data-validation-mode=\"ajax\" class=\"validated-form\">");
		}

		[Test]
		public void modify_the_tag_for_lofi()
		{
			formForMode(ValidationMode.LoFi)
				.ToString().ShouldEqual("<form method=\"post\" action=\"test\" data-validation-mode=\"lofi\" class=\"validated-form\">");
		}

		private HtmlTag formForMode(ValidationMode mode)
		{
			var form = new FormTag("test");
			mode.Modify(form);

			return form;
		}
	}
}