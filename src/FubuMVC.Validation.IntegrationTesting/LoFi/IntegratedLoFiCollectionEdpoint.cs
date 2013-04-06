using System.Collections.Generic;
using FubuValidation;

namespace FubuMVC.Validation.IntegrationTesting.LoFi
{
    public class IntegratedLoFiCollectionEdpoint
    {
        public const string GET = "Collection input data";
        public const string SUCCESS = "Success collection";

        public string get_lofi_collection(LoFiCollectionInput input)
        {
            return GET;
        }

        public string post_lofi_collection(LoFiCollectionInput input)
        {
            return SUCCESS;
        }
    }

    public class LoFiCollectionInput
    {
        public List<LoFiCollectionElement> Collection { get; set; }
    }

    public class LoFiCollectionElement
    {
        [Required]
        public string Name { get; set; }
    }

}