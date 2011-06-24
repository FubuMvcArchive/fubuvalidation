using System;
using FubuCore;
using FubuMVC.Core;
using FubuValidation;

namespace FubuMVC.Validation
{
    public static class FubuRegistryValidationExtensions
    {
        public static void Validation(this FubuRegistry registry)
        {
            registry.Validation(validation => { });
        }

        public static void Validation(this FubuRegistry registry, Action<FubuValidationEngine> configure)
        {
            registry.Validation(ValidationRegistry.BasicRegistry(), validation => { });
        }

        public static void Validation<TRegistry>(this FubuRegistry registry)
            where TRegistry : ValidationRegistry, new()
        {
            registry.Validation(new TRegistry(), validation => { });
        }

        public static void Validation<TRegistry>(this FubuRegistry registry, Action<FubuValidationEngine> configure)
            where TRegistry : ValidationRegistry, new()
        {
            registry.Validation(new TRegistry(), configure);
        }

        public static void Validation(this FubuRegistry registry, ValidationRegistry validationRegistry, Action<FubuValidationEngine> configure)
        {
            var engine = new FubuValidationEngine(validationRegistry);
            configure(engine);
            engine
                .As<IFubuRegistryExtension>()
                .Configure(registry);
        }
    }
}