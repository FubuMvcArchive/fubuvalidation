using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FubuCore;
using FubuCore.Reflection;
using FubuLocalization;

namespace FubuValidation.Fields
{
    public class ListValidationRule : IFieldValidationRule
    {
        private static readonly MethodInfo _method;
        static ListValidationRule()
        {
            _method = ReflectionHelper.GetMethod<IList>(x => x[0]);
        }

        public void Validate(Accessor accessor, ValidationContext context)
        {
            accessor.GetValue(context.Target).As<IList>().Cast<object>().Each((item, i) =>
                {
                    var notification = context.Provider.Validate(item);
                    if (!notification.IsValid())
                    {
                        var indexer = new MethodValueGetter(_method, new object[] { i });
                        var values = new List<IValueGetter>();
                        values.AddRange(accessor.Getters());
                        values.Add(indexer);
                        var childAccessor = new PropertyChain(values.ToArray());
                        context.Notification.AddChild(childAccessor, notification);
                    }
                });
        }
        StringToken IFieldValidationRule.Token { get; set; }
    }
}