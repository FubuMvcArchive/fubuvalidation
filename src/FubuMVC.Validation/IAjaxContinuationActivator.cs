using FubuMVC.Core.Ajax;
using FubuValidation;

namespace FubuMVC.Validation
{
    public interface IAjaxContinuationActivator
    {
        AjaxContinuation Activate(Notification notification);
    }
}