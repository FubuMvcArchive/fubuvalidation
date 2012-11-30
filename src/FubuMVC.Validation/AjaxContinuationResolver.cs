using System.Collections.Generic;
using FubuMVC.Core.Ajax;
using FubuValidation;

namespace FubuMVC.Validation
{
    public interface IAjaxContinuationResolver
    {
        AjaxContinuation Resolve(Notification notification);
    }

    public interface IAjaxContinuationDecorator
    {
        AjaxContinuation Enrich(AjaxContinuation continuation, Notification notification);
    }

     public class AjaxContinuationResolver : IAjaxContinuationResolver
    {
        private readonly IEnumerable<IAjaxContinuationDecorator> _decorators;

        public AjaxContinuationResolver(IEnumerable<IAjaxContinuationDecorator> decorators)
        {
            _decorators = decorators;
        }

        public AjaxContinuation Resolve(Notification notification)
        {
            var continuation = AjaxValidation.BuildContinuation(notification);
            _decorators.Each(d =>
            {
                continuation = d.Enrich(continuation, notification);
            });

            return continuation;
        }
    }
}