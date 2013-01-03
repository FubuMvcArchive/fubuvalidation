using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using FubuValidation;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller
{
	[NotValidated]
	public class IgnoredModel
	{
		[Required]
		public string Name { get; set; } 
	}

	public class IgnoredEndpoint
	{
		private readonly FubuHtmlDocument<IgnoredModel> _page;

		public IgnoredEndpoint(FubuHtmlDocument<IgnoredModel> page)
		{
			_page = page;
		}

		public FubuHtmlDocument<IgnoredModel> get_ignored_model(IgnoredModel model)
		{
			_page.Add(new HtmlTag("h1").Text("Ignored Validation Rules"));
			_page.Add(createForm());
			_page.Add(_page.WriteScriptTags());
			return _page;
		}

		public FubuContinuation post_ignored_model(IgnoredModel model)
		{
			return FubuContinuation.RedirectTo<IgnoredEndpoint>(x => x.get_ignored_hello());
		}

		public string get_ignored_hello()
		{
			return "Hello, world";
		}

		private HtmlTag createForm()
		{
			var form = _page.FormFor<IgnoredModel>();

			form.Append(_page.Edit(x => x.Name));

			form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("Model"));
			form.Id(typeof(IgnoredModel).Name);

			return form;
		}
	}
}