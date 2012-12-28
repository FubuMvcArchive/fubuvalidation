using FubuMVC.Core.UI.Elements;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
    public class MaxValueModifier : InputElementModifier
    {
        protected override void modify(ElementRequest request)
        {
            ForRule<MaxValueFieldRule>(request, rule => request.CurrentTag.Data("max", rule.Bounds));
        }
    }
}