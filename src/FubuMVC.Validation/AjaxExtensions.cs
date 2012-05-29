using System.Collections.Generic;
using FubuValidation;

namespace FubuMVC.Core.Ajax
{
    // Mostly tested via the StandardContinuationDecoratorTester
    public static class CoreAjaxExtensions
    {
         public static AjaxContinuation AsNotification(this AjaxContinuation continuation, Notification notification)
         {
             continuation.Success = notification.IsValid();
             notification
                 .ToValidationErrors()
                 .Each(e => continuation
                                .Errors
                                .Add(new AjaxError
                                {
                                    field = e.field,
									label = e.label,
                                    message = e.message
                                }));

             return continuation;
         }
    }
}