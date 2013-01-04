using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using FubuValidation;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller
{
	public class InlineModel
	{
		[Required]
		public string Name { get; set; }
		[Email]
		public string Email { get; set; }
	}

	public class InlineModelEndpoint
	{
		private readonly FubuHtmlDocument<InlineModel> _page;

		public InlineModelEndpoint(FubuHtmlDocument<InlineModel> page)
		{
			_page = page;
		}

		public FubuHtmlDocument<InlineModel> get_inline_model(InlineModel request)
		{
			_page.Add(new HtmlTag("h1").Text("Inline Errors"));
			_page.Add(createForm());
			_page.Add(_page.WriteScriptTags());
			return _page;
		}

		public FubuContinuation post_inline_model(InlineModel model)
		{
			return FubuContinuation.RedirectTo(new InlineModel(), "GET");
		}

		private HtmlTag createForm()
		{
			var form = _page.FormFor<InlineModel>();
			form.Append(_page.Edit(x => x.Name));
			form.Append(_page.Edit(x => x.Email));
			form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("Model"));
			form.Id("InlineModel");

			return form;
		}
	}
}