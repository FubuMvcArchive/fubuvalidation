using FubuValidation;

namespace FubuMVC.Validation.IntegrationTesting.LoFi
{
    public class IntegratedLoFiEndpoint
    {
        public const string SUCCESS = "Success";

        public string get_lofi(LoFiInput input)
        {
            return "Input data";
        }

        public string post_lofi(LoFiInput input)
        {
            return SUCCESS;
        }
    }

    public class LoFiInput
    {
        [Required]
        public string Name { get; set; }
    }
}