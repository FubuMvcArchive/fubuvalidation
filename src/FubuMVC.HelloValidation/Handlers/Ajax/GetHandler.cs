using FubuMVC.Core.Ajax;
using FubuValidation;

namespace FubuMVC.HelloValidation.Handlers.Ajax
{
    public class GetHandler
    {
        public AjaxContinuation Execute(SomeInputModel input)
        {
            return new AjaxContinuation();
        }
    }

    public class SomeInputModel
    {
        [Required]
        public string RequiredField { get; set; }
    }
}