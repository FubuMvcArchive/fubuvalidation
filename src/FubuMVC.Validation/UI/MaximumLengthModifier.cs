using FubuMVC.Core.UI.Elements;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
    public class MaximumLengthModifier : InputElementModifier
    {
        protected override void modify(ElementRequest request)
        {
            ForRule<MaximumLengthRule>(request, rule => request.CurrentTag.Attr("maxlength", rule.Length));
        }
    }
}