using FubuMVC.Core;

namespace FubuMVC.Validation
{
    public class FubuValidation : IFubuRegistryExtension
    {
        void IFubuRegistryExtension.Configure(FubuRegistry registry)
        {
            registry.Services(x =>
            {
                x.SetServiceIfNone<IAjaxContinuationResolver, AjaxContinuationResolver>();
                x.SetServiceIfNone<IModelBindingErrors, ModelBindingErrors>();
            });

            /*registry
                .Policies
                .Add(new ReorderBehaviorsPolicy
                         {
                             WhatMustBeBefore = node => node is ValidationNode,
                             WhatMustBeAfter = node => node is OutputNode
                         });*/
        }
    }
}