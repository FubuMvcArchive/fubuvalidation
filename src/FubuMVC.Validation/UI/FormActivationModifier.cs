using FubuCore;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Assets;
using FubuMVC.Core.UI.Forms;
using HtmlTags.Conventions;

namespace FubuMVC.Validation.UI
{
    public class FormActivationModifier : ITagModifier<FormRequest>
    {
        public bool Matches(FormRequest token)
        {	
            return true;
        }

        public void Modify(FormRequest request)
        {
            writeScriptRequirements(request);

        	var mode = ValidationMode.For(request);
			mode.Modify(request.CurrentTag);
        }

		private void writeScriptRequirements(FormRequest request)
		{
			request.Services.GetInstance<IAssetRequirements>().Require("ValidationActivator.js");
		}
    }
}