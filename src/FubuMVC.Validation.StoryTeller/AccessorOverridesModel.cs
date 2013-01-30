using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration;
using FubuMVC.Core.UI;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller
{
    public class AccessorOverridesModel
    {
        public int GreaterThanZero { get; set; }
        public int GreaterOrEqualToZero { get; set; }
        public string LongerThanTen { get; set; }
        public string NoMoreThanFiveCharacters { get; set; }
        public string AtLeastFiveButNotTen { get; set; }
        public int GreaterThanFive { get; set; }
        public double LessThanFifteen { get; set; }
        public string Email { get; set; }
        public string Required { get; set; }

        public string Custom { get; set; }
    }

    public class AccessorOverridesModelRules : OverridesFor<AccessorOverridesModel>
    {
        public AccessorOverridesModelRules()
        {
            Property(x => x.GreaterThanZero).GreaterThanZero();
            Property(x => x.GreaterOrEqualToZero).GreaterOrEqualToZero();
            Property(x => x.LongerThanTen).MinimumLength(10);
            Property(x => x.NoMoreThanFiveCharacters).MaximumLength(5);
            Property(x => x.AtLeastFiveButNotTen).RangeLength(5, 10);
            Property(x => x.GreaterThanFive).MinValue(5);
            Property(x => x.LessThanFifteen).MaxValue(15);
            Property(x => x.Email).Email();
            Property(x => x.Required).Required();

            Property(x => x.Custom).Add<UniqueUsernameRule>();
        }
    }

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

            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("Model"));
            form.Id("AccessorOverridesModel");

            return form;
        }
    }
}