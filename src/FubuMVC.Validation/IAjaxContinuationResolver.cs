using FubuMVC.Core.Ajax;
using FubuValidation;

namespace FubuMVC.Validation
{
    // you can plugin here...
    public interface IAjaxContinuationResolver
    {
        AjaxContinuation Resolve(Notification notification);
    }
}