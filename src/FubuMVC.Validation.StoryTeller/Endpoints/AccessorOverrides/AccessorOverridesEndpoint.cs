using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller.Endpoints.AccessorOverrides
{
    public class AccessorOverridesEndpoint
    {
        private readonly FubuHtmlDocument<AccessorOverridesModel> _page;

        public AccessorOverridesEndpoint(FubuHtmlDocument<AccessorOverridesModel> page)
        {
            _page = page;
        }

        public FubuHtmlDocument<AccessorOverridesModel> get_accessor_validation(AccessorOverridesModel request)
        {
            _page.Add(new HtmlTag("h1").Text("Accessor Overrides Validation Rules"));
            _page.Add(createForm());
            _page.Add(_page.WriteScriptTags());
            return _page;
        }

        public FubuContinuation post_accessor_validation(AccessorOverridesModel model)
        {
            return FubuContinuation.RedirectTo(new AccessorOverridesModel(), "GET");
        }

        private HtmlTag createForm()
        {
            var form = _page.FormFor<AccessorOverridesModel>();

            form.Append(_page.Edit(x => x.GreaterThanZero));
            form.Append(_page.Edit(x => x.GreaterOrEqualToZero));
            form.Append(_page.Edit(x => x.LongerThanTen));
            form.Append(_page.Edit(x => x.NoMoreThanFiveCharacters));
            form.Append(_page.Edit(x => x.AtLeastFiveButNotTen));
            form.Append(_page.Edit(x => x.GreaterThanFive));
            form.Append(_page.Edit(x => x.LessThanFifteen));
            form.Append(_page.Edit(x => x.Email));
            form.Append(_page.Edit(x => x.Required));
            form.Append(_page.Edit(x => x.Custom));
            form.Append(_page.Edit(x => x.EmailCustomMessage));
            form.Append(_page.Edit(x => x.Triggered));

            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("Model"));
            form.Id("AccessorOverridesModel");

            return form;
        }
    }
}