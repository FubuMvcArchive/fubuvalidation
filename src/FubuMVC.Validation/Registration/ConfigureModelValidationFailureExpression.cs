using System;
using System.Collections.Generic;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration.ObjectGraph;

namespace FubuMVC.Validation.Registration
{
    public class ConfigureModelValidationFailureExpression
    {
        private readonly Func<ValidationFailure, bool> _predicate;
        private readonly IList<ObjectDef> _policies;

        public ConfigureModelValidationFailureExpression(Func<ValidationFailure, bool> predicate, IList<ObjectDef> policies)
        {
            _predicate = predicate;
            _policies = policies;
        }

        public void RedirectTo<T>()
            where T : class, new()
        {
            RedirectTo(new T());
        }

        public void RedirectTo(object target)
        {
            buildPolicy(FubuContinuation.RedirectTo(target));
        }

        public void TransferTo<T>()
            where T : class, new()
        {
            TransferTo(new T());
        }

        public void TransferTo(object target)
        {
            buildPolicy(FubuContinuation.TransferTo(target));
        }

        public void RedirectBy<TDescriptor>()
            where TDescriptor : class, IFubuContinuationModelDescriptor
        {
            RedirectBy<TDescriptor, FubuRequestInputModelResolver>();
        }

        public void RedirectBy<TDescriptor, TResolver>()
            where TDescriptor : class, IFubuContinuationModelDescriptor
            where TResolver : class, IInputModelResolver
        {
            buildPolicy<TDescriptor, TResolver>(FubuContinuation.RedirectTo);
        }

        public void TransferBy<TDescriptor>()
            where TDescriptor : class, IFubuContinuationModelDescriptor
        {
            TransferBy<TDescriptor, FubuRequestInputModelResolver>();
        }

        public void TransferBy<TDescriptor, TResolver>()
            where TDescriptor : class, IFubuContinuationModelDescriptor
            where TResolver : class, IInputModelResolver
        {
            buildPolicy<TDescriptor, TResolver>(FubuContinuation.TransferTo);
        }

        private void buildPolicy(FubuContinuation continuation)
        {
            var policy = new ObjectDef { Type = typeof(FubuContinuationFailurePolicy) };
            policy.DependencyByValue(typeof (Func<ValidationFailure, bool>), _predicate);
            policy.DependencyByValue(typeof(IFubuContinuationResolver), new ConfiguredFubuContinuationResolver(continuation));

            _policies.Add(policy);
        }

        private void buildPolicy<TDescriptor, TResolver>(Func<object, FubuContinuation> builder)
            where TDescriptor : class, IFubuContinuationModelDescriptor
            where TResolver : class, IInputModelResolver
        {
            var policy = new ObjectDef { Type = typeof(FubuContinuationFailurePolicy) };
            policy.DependencyByValue(typeof(Func<ValidationFailure, bool>), _predicate);

            var modelResolver = new ObjectDef {Type = typeof (FubuContinuationModelResolver)};
            modelResolver.DependencyByType(typeof (IFubuContinuationModelDescriptor), typeof (TDescriptor));
            modelResolver.DependencyByType(typeof (IInputModelResolver), typeof (TResolver));

            var resolver = new ObjectDef {Type = typeof (FubuContinuationResolver)};
            resolver.Dependency(typeof(IFubuContinuationModelResolver), modelResolver);
            resolver.DependencyByValue(typeof(Func<object, FubuContinuation>), builder);

            policy.Dependency(typeof(IFubuContinuationResolver), resolver);
            _policies.Add(policy);
        }
    }
}