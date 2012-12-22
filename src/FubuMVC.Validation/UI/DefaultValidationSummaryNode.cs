using System;
using System.Collections.Generic;
using FubuCore.Descriptions;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Validation.UI
{
    public class DefaultValidationSummaryNode : WriterNode
    {
        protected override ObjectDef toWriterDef()
        {
            return new ObjectDef(typeof(DefaultValidationSummaryWriter));
        }

        protected override void createDescription(Description description)
        {
            description.ShortDescription = "Writes the default html partial for the Validation Summary";
        }

        public override Type ResourceType
        {
            get { return typeof(ValidationSummary); }
        }

        public override IEnumerable<string> Mimetypes
        {
            get
            {
                yield return MimeType.Html.Value;
                yield return MimeType.HttpFormMimetype;
            }
        }
    }
}