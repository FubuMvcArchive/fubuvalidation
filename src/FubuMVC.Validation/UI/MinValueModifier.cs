using FubuMVC.Core.UI.Elements;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
    public class MinValueModifier : InputElementModifier
    {
        protected override void modify(ElementRequest request)
        {
            ForRule<MinValueFieldRule>(request, rule => request.CurrentTag.Data("min", rule.Bounds));
        }
    }
}