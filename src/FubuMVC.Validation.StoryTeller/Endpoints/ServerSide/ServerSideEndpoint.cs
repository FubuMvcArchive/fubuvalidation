using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller
{
    public class ServerSideEndpoint
    {
        private readonly FubuHtmlDocument<ServerSideModel> _page;

        public ServerSideEndpoint(FubuHtmlDocument<ServerSideModel> page)
        {
            _page = page;
        }

        public FubuHtmlDocument<ServerSideModel> get_server_side(ServerSideModel model)
        {
            _page.Add(new HtmlTag("h1").Text("Server Side Rules"));
            _page.Add(createForm());
            _page.Add(_page.WriteScriptTags());
            return _page;
        }

        public FubuContinuation post_server_side(ServerSideModel model)
        {
            return FubuContinuation.RedirectTo(new ServerSideModel(), "GET");
        }

        private HtmlTag createForm()
        {
            var form = _page.FormFor<ServerSideModel>();

            form.Append(_page.Edit(x => x.Value));

            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("Model"));
            form.Id("ServerSideModel");

            return form;
        }
    }
}