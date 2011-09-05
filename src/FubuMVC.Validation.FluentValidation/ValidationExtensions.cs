using FubuMVC.Core;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuValidation;
using FubuValidation.FluentValidation;

namespace FubuMVC.Validation.FluentValidation
{
    public static class ValidationExtensions
    {
        public static void WithFluentValidation(this FubuRegistry registry)
        {
            registry
                .Services(x =>
                              {
                                  x.SetServiceIfNone<INotificationFiller, NotificationFiller>();
                                  x.SetServiceIfNone<INotificationMessageProvider, NotificationMessageProvider>();
                                  x.AddService(typeof(IValidationSource), new ObjectDef(typeof(FluentValidationSource)));
                              });
        }
    }
}