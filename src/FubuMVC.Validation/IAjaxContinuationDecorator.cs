using FubuMVC.Core.Ajax;
using FubuValidation;

namespace FubuMVC.Validation
{
    public interface IAjaxContinuationDecorator
    {
        AjaxContinuation Enrich(AjaxContinuation continuation, Notification notification);
    }
}