using FubuMVC.Core.UI.Elements;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
    public class MinimumLengthModifier : InputElementModifier
    {
        protected override void modify(ElementRequest request)
        {
            ForRule<MinimumLengthRule>(request, rule => request.CurrentTag.Data("minlength", rule.Length));
        }
    }
}