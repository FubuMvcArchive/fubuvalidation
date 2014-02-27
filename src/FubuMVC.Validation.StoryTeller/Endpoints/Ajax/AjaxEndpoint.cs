using FubuMVC.Core.Ajax;
using FubuMVC.Core.Assets;
using FubuMVC.Core.UI;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller.Endpoints.Ajax
{
    public class AjaxEndpoint
    {
        private readonly FubuHtmlDocument<AjaxModel> _page;

        public AjaxEndpoint(FubuHtmlDocument<AjaxModel> page)
        {
            _page = page;
        }

        public FubuHtmlDocument<AjaxModel> get_ajax(AjaxModel request)
        {
            _page.Add(new HtmlTag("h1").Text("Ajax Form"));
            _page.Asset("ajaxTester.js");
            _page.Add(createForm());
            _page.Add(new LiteralTag(_page.EndForm().ToString()));
            _page.Add(_page.WriteScriptTags());
            return _page;
        }

        public AjaxContinuation post_ajax(AjaxModel user)
        {
            var continuation = AjaxContinuation.Successful();
            continuation.Message = "User created: " + user.Name;

            return continuation;
        }

        private HtmlTag createForm()
        {
            var form = _page.FormFor<AjaxModel>();

            form.Append(_page.Edit(x => x.Name));

            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("Submit"));
            form.Id("AjaxForm");

            return form;
        }
    }
}