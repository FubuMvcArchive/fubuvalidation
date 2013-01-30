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
	        return true;
        }

        public void Modify(FormRequest request)
        {
			if (!request.Chain.ValidationNode().Contains(RenderingStrategies.Summary))
			{
				return;
			}

            var summary = request.Services.GetInstance<IPartialInvoker>().Invoke<ValidationSummary>();
            request.CurrentTag.InsertFirst(new LiteralTag(summary));
        }
    }
}