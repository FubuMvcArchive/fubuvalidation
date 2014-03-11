using FubuValidation;

namespace FubuMVC.Validation.IntegrationTesting.LoFi_diff_models
{
    public class IntegratedLoFi_diff_models_Endpoint
    {
        public const string GET = "Input data";
        public const string SUCCESS = "Success";

        public string get_lofi(LoFiInput_get input)
        {
            return GET;
        }

        public string post_lofi(LoFiInput_post input)
        {
            return SUCCESS;
        }
    }

    public class LoFiInput_get
    {
        public string Name { get; set; }
    }


    public class LoFiInput_post : IOverrideInputModelForWhenValidationFails<LoFiInput_post>
    {
        [Required]
        public string Name { get; set; }

        public object GetInputModelForValidationFail(LoFiInput_post inputModel)
        {
            return new LoFiInput_get(){Name = inputModel.Name};
        }
    }
}