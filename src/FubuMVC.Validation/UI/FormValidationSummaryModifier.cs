using System.Linq;
using FubuMVC.Core.UI;
using FubuMVC.Core.UI.Forms;
using HtmlTags;
using HtmlTags.Conventions;

namespace FubuMVC.Validation.UI
{
    public class FormValidationSummaryModifier : ITagModifier<FormRequest>
    {
        public bool Matches(FormRequest token)
        {
            return token.Chain.ValidationNode().Contains(RenderingStrategies.Summary);
        }

        public void Modify(FormRequest request)
        {
            var summary = request.Services.GetInstance<IPartialInvoker>().Invoke<ValidationSummary>();
            request.CurrentTag.InsertFirst(new LiteralTag(summary));
        }
    }
}