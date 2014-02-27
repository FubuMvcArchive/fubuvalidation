using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller.Endpoints.ClassValidation
{
    public class ClassValidationEndpoint
    {
        private readonly FubuHtmlDocument<ClassValidationModel> _page;

        public ClassValidationEndpoint(FubuHtmlDocument<ClassValidationModel> page)
        {
            _page = page;
        }

        public FubuHtmlDocument<ClassValidationModel> get_class_validation(ClassValidationModel request)
        {
            _page.Add(new HtmlTag("h1").Text("Class Validation Rules"));
            _page.Add(createForm());
            _page.Add(_page.WriteScriptTags());
            return _page;
        }

        public FubuContinuation post_class_validation(ClassValidationModel user)
        {
            return FubuContinuation.RedirectTo(new ClassValidationModel(), "GET");
        }

        private HtmlTag createForm()
        {
            var form = _page.FormFor<ClassValidationModel>();

            form.Append(_page.Edit(x => x.GreaterThanZero));
            form.Append(_page.Edit(x => x.GreaterOrEqualToZero));
            form.Append(_page.Edit(x => x.LongerThanTen));
            form.Append(_page.Edit(x => x.NoMoreThanFiveCharacters));
            form.Append(_page.Edit(x => x.AtLeastFiveButNotTen));
            form.Append(_page.Edit(x => x.GreaterThanFive));
            form.Append(_page.Edit(x => x.LessThanFifteen));
            form.Append(_page.Edit(x => x.Email));
            form.Append(_page.Edit(x => x.Required));
            form.Append(_page.Edit(x => x.Regex));
            form.Append(_page.Edit(x => x.Custom));
            form.Append(_page.Edit(x => x.Password));
            form.Append(_page.Edit(x => x.ConfirmPassword));
            form.Append(_page.Edit(x => x.ConfirmEmail));


            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("Model"));
            form.Id(typeof(ClassValidationModel).Name);

            return form;
        }
    }
}