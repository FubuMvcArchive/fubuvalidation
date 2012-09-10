using System;
using System.Net;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Querying;
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
        private readonly IChainResolver _resolver;
        private readonly IUrlRegistry _registry;
        private readonly IFubuRequest _request;
        private readonly IOutputWriter _writer;

        public ValidationContinuationHandler(IUrlRegistry registry, IOutputWriter writer, IFubuRequest request, IPartialFactory factory, IChainResolver resolver)
        {
            _registry = registry;
            _writer = writer;
            _request = request;
            _factory = factory;
            _resolver = resolver;
        }


        public void InvokeNextBehavior()
        {
            // no-op
        }

        public void RedirectTo(object input, string httpMethodOrCategory = null)
        {
            var url = input as string ?? _registry.UrlFor(input);
            _writer.RedirectToUrl(url);
        }

        public void RedirectToCall(ActionCall call, string httpMethodOrCategory = null)
        {
            var url = _registry.UrlFor(call.HandlerType, call.Method);
            _writer.RedirectToUrl(url);
        }

        public void TransferTo(object input, string httpMethodOrCategory = null)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            _request.SetObject(input);

            var chain = _resolver.FindUnique(input, httpMethodOrCategory);

            _factory
                .BuildPartial(chain)
                .InvokePartial();
        }

        public void TransferToCall(ActionCall call, string httpMethodOrCategory = null)
        {
            var chain = _resolver.Find(call.HandlerType, call.Method, httpMethodOrCategory);

            _factory
                .BuildPartial(chain)
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