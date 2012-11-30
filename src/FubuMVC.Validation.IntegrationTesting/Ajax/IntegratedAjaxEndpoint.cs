using FubuMVC.Core.Ajax;
using FubuValidation;

namespace FubuMVC.Validation.IntegrationTesting.Ajax
{
    public class IntegratedAjaxEndpoint
    {
         public AjaxContinuation post_ajax(AjaxRequest request)
         {
             return AjaxContinuation.Successful();
         }
    }

    public class AjaxRequest
    {
        [Required]
        public string Name { get; set; }
    }
}