using System;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Validation
{
    public class FubuRequestInputModelResolver : IInputModelResolver
    {
        private readonly IFubuRequest _request;

        public FubuRequestInputModelResolver(IFubuRequest request)
        {
            _request = request;
        }

        public object Resolve(Type destinationType, Type sourceType, object source)
        {
            return _request.Get(destinationType);
        }
    }
}