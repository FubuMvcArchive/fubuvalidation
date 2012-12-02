using FubuMVC.Core.Ajax;

namespace FubuMVC.HelloValidation.Ajax
{
    public class AjaxEndpoint
    {
        public TestItem get_ajax_test(TestItem item)
        {
            return new TestItem();
        }

        public AjaxContinuation post_ajax_test(TestItem item)
        {
            return AjaxContinuation.Successful();
        }
    }
}