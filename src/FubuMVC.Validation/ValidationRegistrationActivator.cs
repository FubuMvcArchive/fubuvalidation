using System;
using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using FubuCore;
using FubuMVC.Core.Registration;
using FubuValidation;

namespace FubuMVC.Validation
{
    public class ValidationRegistrationActivator : IActivator
    {
        private readonly ValidationGraph _graph;

        public ValidationRegistrationActivator(ValidationGraph graph)
        {
            _graph = graph;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            var types = Types();
            types
                .TypesMatching(IsValidationRegistration)
                .Each(type =>
                {
                    var registration = Activator.CreateInstance(type).As<IValidationRegistration>();
                    registration.Register(_graph);
                });
        }

        public static bool IsValidationRegistration(Type type)
        {
            return type.IsConcreteTypeOf<IValidationRegistration>()
                && type.IsConcreteWithDefaultCtor()
                && !type.IsOpenGeneric();
        }

        public virtual TypePool Types()
        {
            return TypePool.AppDomainTypes();
        }
    }
}