using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller
{
    public class IntegrationEndpoint
    {
        private readonly FubuHtmlDocument<IntegrationModel> _page;

        public IntegrationEndpoint(FubuHtmlDocument<IntegrationModel> page)
        {
            _page = page;
        }

        public FubuHtmlDocument<IntegrationModel> get_integration(User request)
        {
            _page.Add(new HtmlTag("h1").Text("All The Rules"));
            _page.Add(createForm());
            _page.Add(_page.WriteScriptTags());
            return _page;
        }

        public FubuContinuation post_integration(IntegrationModel user)
        {
            return FubuContinuation.RedirectTo(new IntegrationModel(), "GET");
        }

        private HtmlTag createForm()
        {
            var form = _page.FormFor<IntegrationModel>();
            
            form.Append(_page.Edit(x => x.Numeric));
            form.Append(_page.Edit(x => x.FubuDate));
            form.Append(_page.Edit(x => x.StandardDate));
            form.Append(_page.Edit(x => x.GreaterThanZero));
            form.Append(_page.Edit(x => x.GreaterOrEqualToZero));
            form.Append(_page.Edit(x => x.LongerThanTen));
            form.Append(_page.Edit(x => x.NoMoreThanFiveCharacters));
            form.Append(_page.Edit(x => x.AtLeastFiveButNotTen));
            form.Append(_page.Edit(x => x.GreaterThanFive));
            form.Append(_page.Edit(x => x.LessThanFifteen));
            form.Append(_page.Edit(x => x.Email));
            form.Append(_page.Edit(x => x.Required));
            
            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("Model"));
            form.Id("IntegrationModel");

            return form;
        }
    }
}