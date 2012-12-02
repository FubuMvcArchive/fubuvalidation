using System.Net;
using FubuMVC.Core.Runtime;
using FubuValidation;

namespace FubuMVC.Validation
{
    public interface IAjaxValidationFailureHandler
    {
        void Handle(Notification notification);
    }

    public class AjaxValidationFailureHandler : IAjaxValidationFailureHandler
    {
        private readonly IAjaxContinuationResolver _resolver;
        private readonly IOutputWriter _output;
        private readonly IFubuRequest _request;

        public AjaxValidationFailureHandler(IAjaxContinuationResolver resolver, IOutputWriter output, IFubuRequest request)
        {
            _resolver = resolver;
            _output = output;
            _request = request;
        }

        public void Handle(Notification notification)
        {
            var continuation = _resolver.Resolve(notification);
            
            _output.WriteResponseCode(HttpStatusCode.BadRequest);

            _request.Set(continuation);
        }
    }
}