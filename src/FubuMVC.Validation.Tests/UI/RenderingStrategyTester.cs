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
	public class RenderingStrategyTester
	{
        private FormRequest theRequest;

		[SetUp]
		public void SetUp()
		{
			theRequest = new FormRequest(new ChainSearch {Type = typeof (object)}, new object());
			var theForm = new FormTag("test");
			theRequest.ReplaceTag(theForm);
		}

		[Test]
		public void summary_strategy_adds_the_validation_summary_attribute()
		{
			RenderingStrategy.Summary.Modify(theRequest);
			theRequest.CurrentTag.ToString().ShouldEqual("<form method=\"post\" action=\"test\" data-validation-summary=\"true\">");
		}

		[Test]
		public void highlight_strategy_adds_the_validation_highlight_attribute()
		{
			RenderingStrategy.Highlight.Modify(theRequest);
			theRequest.CurrentTag.ToString().ShouldEqual("<form method=\"post\" action=\"test\" data-validation-highlight=\"true\">");
		}

		[Test]
		public void inline_strategy_adds_the_validation_inline_attribute()
		{
			RenderingStrategy.Inline.Modify(theRequest);
			theRequest.CurrentTag.ToString().ShouldEqual("<form method=\"post\" action=\"test\" data-validation-inline=\"true\">");
		}
	}

	[TestFixture]
	public class RenderingStrategiesTester
	{
		private RenderingStrategyRegistry theStrategies;

		[SetUp]
		public void SetUp()
		{
			theStrategies = new RenderingStrategyRegistry();
		}

		[Test]
		public void no_duplicates()
		{
			var s1 = MockRepository.GenerateStub<IRenderingStrategy>();
			theStrategies.RegisterStrategy(s1);

			theStrategies.All().ShouldHaveTheSameElementsAs(s1);
		}

		[Test]
		public void clears_the_strategies()
		{
			var s1 = MockRepository.GenerateStub<IRenderingStrategy>();
			var s2 = MockRepository.GenerateStub<IRenderingStrategy>();
			
			theStrategies.RegisterStrategy(s1);
			theStrategies.RegisterStrategy(s2);

			theStrategies.Clear();

			theStrategies.All().ShouldHaveCount(0);
		}

		[Test]
		public void defaults()
		{
			var strategies = RenderingStrategyRegistry.Default();
			strategies.All().ShouldHaveTheSameElementsAs(RenderingStrategy.Summary, RenderingStrategy.Highlight);
		}
	}
}