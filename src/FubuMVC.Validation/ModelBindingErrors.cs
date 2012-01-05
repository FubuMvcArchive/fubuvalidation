using System.Collections.Generic;
using System.Reflection;
using FubuMVC.Core.Runtime;
using FubuValidation;

namespace FubuMVC.Validation
{
    public interface IModelBindingErrors
    {
        void AddAnyErrors<T>(Notification notification);
    }

    public class ModelBindingErrors : IModelBindingErrors
    {
        private readonly IFubuRequest _request;

        public ModelBindingErrors(IFubuRequest request)
        {
            _request = request;
        }

        public void AddAnyErrors<T>(Notification notification)
        {
            var problems = _request.ProblemsFor<T>();
            problems.Each(problem =>
            {
                problem.Properties.Each(prop => notification.RegisterMessage((PropertyInfo) prop, ValidationKeys.INVALID_FORMAT));
            });
        }
    }
}