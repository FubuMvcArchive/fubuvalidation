using FubuCore;
using FubuMVC.Core.Assets;
using FubuMVC.Core.UI.Forms;
using HtmlTags.Conventions;

namespace FubuMVC.Validation.UI
{
    public class FormValidationModifier : ITagModifier<FormRequest>
    {
        public bool Matches(FormRequest token)
        {
            return true;
        }

        public void Modify(FormRequest request)
        {
            var validation = request.Chain.ValidationNode();
            if (validation.IsEmpty())
            {
                return;
            }

            var filter = request.Services.GetInstance<ValidationSettings>().As<IFormActivationFilter>();
            if (filter.ShouldActivate(request.Chain))
            {
                writeScriptRequirements(request);
            }

            var node = validation.As<IValidationNode>();
            node.Modify(request);

            var options = ValidationOptions.For(request);
            request.CurrentTag.Data(ValidationOptions.Data, options);

            request.CurrentTag.AddClass("validated-form");
        }

        private void writeScriptRequirements(FormRequest request)
        {
            request.Services.GetInstance<IAssetRequirements>().Require("ValidationActivator.js");
        }
    }
}