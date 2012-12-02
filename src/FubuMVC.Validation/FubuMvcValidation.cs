using FubuMVC.Core;

namespace FubuMVC.Validation
{
    public class FubuMvcValidation : IFubuRegistryExtension
    {
        void IFubuRegistryExtension.Configure(FubuRegistry registry)
        {
            registry.Services(x =>
            {
                x.SetServiceIfNone<IAjaxContinuationResolver, AjaxContinuationResolver>();
                x.SetServiceIfNone<IModelBindingErrors, ModelBindingErrors>();
                x.SetServiceIfNone<IAjaxValidationFailureHandler, AjaxValidationFailureHandler>();
                x.SetServiceIfNone(typeof (IValidationFilter<>), typeof (ValidationFilter<>));
            });

            registry.Policies.Add<ValidationConvention>();
        }
    }
}