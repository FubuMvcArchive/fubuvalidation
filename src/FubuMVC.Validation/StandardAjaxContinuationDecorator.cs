using System.Collections.Generic;
using FubuMVC.Core.Ajax;
using FubuValidation;

namespace FubuMVC.Validation
{
    public class StandardAjaxContinuationDecorator : IAjaxContinuationDecorator
    {
        public AjaxContinuation Enrich(AjaxContinuation continuation, Notification notification)
        {
            continuation.Success = notification.IsValid();
            notification
                .ToValidationErrors()
                .Each(e => continuation
                               .Errors
                               .Add(new AjaxError
                                        {
                                            field = e.field,
                                            message = e.message
                                        }));

            return continuation;
        }
    }
}