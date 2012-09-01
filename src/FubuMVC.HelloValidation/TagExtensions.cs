using FubuLocalization;
using HtmlTags;

namespace FubuMVC.HelloValidation
{
    public static class TagExtensions
    {
        public static FormTag WithValidationSummary<T>(this FormTag form)
        {
            form.Id(typeof(T).Name);
            var summary = new HtmlTag("div")
                .AddClasses("alert", "alert-error", "validation-container")
                .Append(new HtmlTag("p").Text(HelloValidationKeys.Summary))
                .Append(new HtmlTag("ul").AddClass("validation-summary"))
                .Style("display", "none");
            form.Append(summary);
            return form;
        }
    }

    public class HelloValidationKeys : StringToken
    {
        public static readonly HelloValidationKeys Summary = new HelloValidationKeys("There are errors with the information you provided.");

        private HelloValidationKeys(string defaultValue)
            : base(null, defaultValue, namespaceByType: true)
        {
        }
    }
}