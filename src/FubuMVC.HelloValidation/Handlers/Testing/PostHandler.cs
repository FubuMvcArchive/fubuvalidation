using FubuMVC.Core.Ajax;

namespace FubuMVC.HelloValidation.Handlers.Testing
{
    public class PostHandler
    {
        public AjaxContinuation Execute(TestItem item)
        {
            return AjaxContinuation.Successful();
        }
    }
}