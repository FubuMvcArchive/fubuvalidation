using FubuCore.Reflection;
using FubuMVC.Core.UI.Elements;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
    public class LocalizationAnnotationStrategy : IValidationAnnotationStrategy
    {
        public const string LocalizationKey = "localization";

        public bool Matches(IFieldValidationRule rule)
        {
            // TODO -- We might need to make this smarter
            return rule.Token != null && !rule.GetType().HasAttribute<IgnoreClientLocalizationAttribute>();
        }

        public void Modify(ElementRequest request, IFieldValidationRule rule)
        {
            var messages = request.CurrentTag.Data(LocalizationKey) as ElementLocalizationMessages;
            if (messages == null)
            {
                messages = new ElementLocalizationMessages();
                request.CurrentTag.Data(LocalizationKey, messages);
            }

            messages.Add(rule);
        }
    }
}