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
			if(validation.IsEmpty())
			{
				return;
			}

            writeScriptRequirements(request);
	        validation.Modify(request);

	        request.CurrentTag.AddClass("validated-form");
        }

		private void writeScriptRequirements(FormRequest request)
		{
			request.Services.GetInstance<IAssetRequirements>().Require("ValidationActivator.js");
		}
    }
}