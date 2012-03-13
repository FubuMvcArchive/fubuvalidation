using FubuMVC.Core.Ajax;
using FubuValidation;

namespace FubuMVC.Validation
{
    public class StandardAjaxContinuationDecorator : IAjaxContinuationDecorator
    {
        public AjaxContinuation Enrich(AjaxContinuation continuation, Notification notification)
        {
            return continuation.AsNotification(notification);
        }
    }
}