using FubuMVC.Core;

namespace FubuMVC.Validation.FluentValidation
{
    public class FluentValidationActivator : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.WithFluentValidation();
        }
    }
}