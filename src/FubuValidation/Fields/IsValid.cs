using System.Linq;
using FubuCore.Reflection;

namespace FubuValidation.Fields
{
    public class IsValid : IFieldRuleCondition
    {
        public bool Matches(Accessor accessor, ValidationContext context)
        {
            return !context.Notification.MessagesFor(accessor).Any();
        }
    }
}