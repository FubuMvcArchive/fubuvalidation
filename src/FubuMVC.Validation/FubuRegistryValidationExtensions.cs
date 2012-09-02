using System;
using FubuMVC.Core;

namespace FubuMVC.Validation
{
    public static class FubuRegistryValidationExtensions
    {
        [Obsolete("Use Import<FubuValidation>() instead.  Will be removed before 1.0")]
        public static void Validation(this FubuRegistry registry)
        {
            registry.Import<FubuValidation>();
        }

        [Obsolete("Use Import<FubuValidation>() instead.  Will be removed before 1.0")]
        public static void Validation(this FubuRegistry registry, Action<FubuValidation> configure)
        {
            registry.Import(configure);
        }
    }
}