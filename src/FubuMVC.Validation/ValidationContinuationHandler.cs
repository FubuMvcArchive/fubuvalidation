using System;
using System.Net;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;

namespace FubuMVC.Validation
{
    public interface IValidationContinuationHandler
    {
        void Handle();
    }

    public class ValidationContinuationHandler : IContinuationDirector, IValidationContinuationHandler
    {
        private readonly IPartialFactory _factory;
        private readonly IUrlRegistry _registry;
        private readonly IFubuRequest _request;
        private readonly IOutputWriter _writer;

        public ValidationContinuationHandler(IUrlRegistry registry, IOutputWriter writer, IFubuRequest request,
            IPartialFactory factory)
        {
            _registry = registry;
            _writer = writer;
            _request = request;
            _factory = factory;
        }


        public void InvokeNextBehavior()
        {
            // no-op
        }

        public void RedirectTo(object input)
        {
            var url = input as string ?? _registry.UrlFor(input);
            _writer.RedirectToUrl(url);
        }

        public void RedirectToCall(ActionCall call)
        {
            var url = _registry.UrlFor(call.HandlerType, call.Method);
            _writer.RedirectToUrl(url);
        }

        public void TransferTo(object input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            _request.SetObject(input);

            _factory
                .BuildPartial(input.GetType())
                .InvokePartial();
        }

        public void TransferToCall(ActionCall call)
        {
            _factory
                .BuildPartial(call)
                .InvokePartial();
        }

        public void EndWithStatusCode(HttpStatusCode code)
        {
            // no-op
        }

        public void Handle()
        {
            _request
                .Get<FubuContinuation>()
                .Process(this);
        }
    }
}