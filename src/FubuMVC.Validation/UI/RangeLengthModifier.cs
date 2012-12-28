using System.Collections.Generic;
using FubuMVC.Core.UI.Elements;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
    public class RangeLengthModifier : InputElementModifier
    {
        protected override void modify(ElementRequest request)
        {
            ForRule<RangeLengthFieldRule>(request, rule => request.CurrentTag.Data("rangelength", rule.ToValues()));
        }
    }
}