using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using FubuMVC.Validation.StoryTeller.Endpoints.Integration;
using FubuValidation;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller
{
    public class SelectTagModel
    {
        [Required]
        public SimpleList List { get; set; }
    }

    public class SelectTagEndpoint
    {
        private readonly FubuHtmlDocument<SelectTagModel> _page;

        public SelectTagEndpoint(FubuHtmlDocument<SelectTagModel> page)
        {
            _page = page;
        }

        public FubuHtmlDocument<SelectTagModel> get_select(SelectTagModel model)
        {
            _page.Add(new HtmlTag("h1").Text("All The Rules"));
            _page.Add(createForm());
            _page.Add(_page.WriteScriptTags());
            return _page;
        }

        public FubuContinuation post_integration(SelectTagModel model)
        {
            return FubuContinuation.RedirectTo(new IntegrationModel(), "GET");
        }

        private HtmlTag createForm()
        {
            var form = _page.FormFor<SelectTagModel>();

            form.Append(_page.Edit(x => x.List));

            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("Model"));
            form.Id("SelectTagModel");

            return form;
        }
    }
}