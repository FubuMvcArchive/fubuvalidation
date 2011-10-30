using System.Collections.Generic;
using FubuMVC.Core.Ajax;
using FubuValidation;

namespace FubuMVC.Validation
{
    public class AjaxContinuationResolver : IAjaxContinuationResolver
    {
        private readonly IAjaxContinuationActivator _activator;
        private readonly IEnumerable<IAjaxContinuationDecorator> _decorators;

        public AjaxContinuationResolver(IAjaxContinuationActivator activator, IEnumerable<IAjaxContinuationDecorator> decorators)
        {
            _activator = activator;
            _decorators = decorators;
        }

        public AjaxContinuation Resolve(Notification notification)
        {
            var continuation = _activator.Activate(notification);
            _decorators
                .Each(d =>
                          {
                              continuation = d.Enrich(continuation, notification);
                          });

            return continuation;
        }
    }
}