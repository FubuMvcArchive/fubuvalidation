using System.Collections.Generic;
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

        public AjaxContinuation post_ajax_collection(AjaxCollectionRequest request)
        {
            return AjaxContinuation.Successful();
        }

    }

    public class AjaxRequest
    {
        [Required]
        public string Name { get; set; }
    }

    public class AjaxCollectionRequest
    {
        public List<CollectionItem> Collection { get; set; }
    }

    public class CollectionItem
    {
        [Required]
        public string Name { get; set; }
    }


}