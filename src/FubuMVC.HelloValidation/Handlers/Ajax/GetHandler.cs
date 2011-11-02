using FubuMVC.Core.Ajax;
using FubuValidation;

namespace FubuMVC.HelloValidation.Handlers.Ajax
{
    public class GetHandler
    {
        public AjaxContinuation Execute(SomeInputModel input)
        {
            // something happened and it was successful
            return new AjaxContinuation
                       {
                           Success = true
                       };
        }
    }

    public class SomeInputModel
    {
        [Required]
        public string RequiredField { get; set; }
    }
}