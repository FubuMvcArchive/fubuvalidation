using FubuCore;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Assets;
using FubuMVC.Core.UI.Forms;
using HtmlTags.Conventions;

namespace FubuMVC.Validation.UI
{
    public class FormValidationMode : ITagModifier<FormRequest>
    {
        public static readonly string LoFi = "lofi";
        public static readonly string Ajax = "ajax";


        public bool Matches(FormRequest token)
        {
            return true;
        }

        public void Modify(FormRequest request)
        {
            if(request.Chain.ResourceType() == null)
            {
                return;
            }

            request.Services.GetInstance<IAssetRequirements>().Require("ValidationActivator.js");

            var mode = ModeFor(request);
            request.CurrentTag.Data("validation-mode", mode).AddClass("validated-form");
        }

        public static string ModeFor(FormRequest request)
        {
            if(request.Chain.ResourceType().CanBeCastTo<AjaxContinuation>())
            {
                return Ajax;
            }

            return LoFi;
        }
    }
}