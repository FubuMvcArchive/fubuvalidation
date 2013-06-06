using FubuLocalization;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using FubuValidation;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller
{
    public class ClassValidationModel
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
		public string Regex { get; set; }

        public string Custom { get; set; }

		public string Password { get; set; }
		public string ConfirmPassword { get; set; }

		public string ConfirmEmail { get; set; }
    }

	// SAMPLE: ClassValidationRules
    public class ClassValidationModelRules : ClassValidationRules<ClassValidationModel>
    {
        public ClassValidationModelRules()
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
	        Property(x => x.Regex).RegEx("[a-zA-Z0-9]+$");

            Property(x => x.Custom).Rule<UniqueUsernameRule>();

	        Property(x => x.Password)
				.Matches(x => x.ConfirmPassword)
		        .ReportErrorsOn(x => x.Password)
		        .ReportErrorsOn(x => x.ConfirmPassword);

	        Property(x => x.Email)
		        .Matches(x => x.ConfirmEmail)
		        .ReportErrorsOn(x => x.ConfirmEmail)
		        .UseToken(StringToken.FromKeyString("Test:Keys", "Emails must match"));
        }
    }
	// ENDSAMPLE

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