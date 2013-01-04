using System;
using System.Linq.Expressions;
using System.Reflection;
using FubuCore.Reflection;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuMVC.Validation.UI;

namespace FubuMVC.Validation
{
	public class ValidationActionFilter : ActionFilter, ValidationNode
	{
		public ValidationActionFilter(Type handlerType, MethodInfo method) : base(handlerType, method)
		{
			Strategies = RenderingStrategyRegistry.Default();
		}

		public ValidationMode Mode
		{
			get { return ValidationMode.LoFi; }
		}

		public RenderingStrategyRegistry Strategies { get; private set; }

		public static ActionFilter ValidationFor<T>(Expression<Func<T, object>> method)
		{
			return new ValidationActionFilter(typeof(T), ReflectionHelper.GetMethod<T>(method));
		}
	}

    public class ValidationActionFilter<T>
    {
        private readonly IValidationFilter<T> _filter;
        private readonly IFubuRequest _request;

        public ValidationActionFilter(IValidationFilter<T> filter, IFubuRequest request)
        {
            _filter = filter;
            _request = request;
        }

        public FubuContinuation Validate(T input)
        {
            var notification = _filter.Validate(input);
            if(notification.IsValid())
            {
                return FubuContinuation.NextBehavior();
            }

            _request.Set(notification);
            return FubuContinuation.TransferTo(input, categoryOrHttpMethod: "GET");
        }
    }
}