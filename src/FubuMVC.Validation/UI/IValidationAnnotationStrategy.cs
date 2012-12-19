using FubuMVC.Core.UI.Elements;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
    public interface IValidationAnnotationStrategy
    {
        bool Matches(IFieldValidationRule rule);
        void Modify(ElementRequest request, IFieldValidationRule rule);
    }
}