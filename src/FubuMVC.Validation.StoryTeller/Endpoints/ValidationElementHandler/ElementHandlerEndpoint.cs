using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller.Endpoints.ValidationElementHandler
{
    public class ElementHandlerEndpoint
    {
        private readonly FubuHtmlDocument<ElementHandler> _page;

        public ElementHandlerEndpoint(FubuHtmlDocument<ElementHandler> page)
        {
            _page = page;
        }

        public FubuHtmlDocument<ElementHandler> get_elementHandlers_create(ElementHandler request)
        {
            _page.Add(new HtmlTag("h1").Text("ValidationElementHandler test cases"));
            _page.Add(CreateForm());
            _page.Add(new TextboxTag().Attr("name", "NoValidation"));
            _page.Add(_page.WriteScriptTags());
            return _page;
        }

        public FubuContinuation post_elementHandlers_create(ElementHandler user)
        {
            return FubuContinuation.RedirectTo(new ElementHandler());
        }

        private HtmlTag CreateForm()
        {
            return _page.FormFor<ElementHandler>(form =>
            {
                form.Append(_page.Edit(x => x.AjaxValueFast));
                form.Append(_page.Edit(x => x.AjaxValueSlow));
                form.Append(_page.Edit(x => x.AjaxValueReallySlow));
                form.Append(_page.Edit(x => x.SynchronousValue));
                form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("ElementHandler"));
                form.Id("ElementHandlerForm");
            });
        }
    }
}