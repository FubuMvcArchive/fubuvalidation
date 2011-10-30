using FubuMVC.Core.Ajax;
using FubuValidation;

namespace FubuMVC.Validation
{
    public class AjaxContinuationActivator : IAjaxContinuationActivator
    {
        public AjaxContinuation Activate(Notification notification)
        {
            return new AjaxContinuation();
        }
    }
}