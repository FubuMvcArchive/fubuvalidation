using System.Collections.Generic;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;
using FubuValidation;
using HtmlTags;

namespace FubuMVC.Validation.UI
{
    public class DefaultValidationSummaryWriter : IMediaWriter<ValidationSummary>
    {
        private readonly IOutputWriter _writer;

        public DefaultValidationSummaryWriter(IOutputWriter writer)
        {
            _writer = writer;
        }

        public void Write(string mimeType, ValidationSummary resource)
        {
            _writer.WriteHtml(BuildSummary());
        }

        public virtual HtmlTag BuildSummary()
        {
            return new HtmlTag("div")
                .AddClasses("alert", "alert-error", "validation-container")
                .Append(new HtmlTag("p").Text(ValidationKeys.Summary))
                .Append(new HtmlTag("ul").AddClass("validation-summary"))
                .Style("display", "none");
        }

        public IEnumerable<string> Mimetypes
        {
            get { yield return MimeType.Html.Value; }
        }
    }
}