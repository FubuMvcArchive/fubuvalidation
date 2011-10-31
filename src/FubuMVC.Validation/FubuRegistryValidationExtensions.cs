using System;
using FubuCore;
using FubuMVC.Core;

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
            var engine = new FubuValidationEngine();
            configure(engine);
            engine
                .As<IFubuRegistryExtension>()
                .Configure(registry);
        }
    }
}