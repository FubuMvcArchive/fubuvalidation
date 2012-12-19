using System.Collections.Generic;
using FubuMVC.Core.UI.Elements;
using FubuValidation;

namespace FubuMVC.Validation.UI
{
    public class FieldValidationElementModifier : IElementModifier
    {
        public bool Matches(ElementRequest token)
        {
            return true;
        }

        public void Modify(ElementRequest request)
        {
            var tag = request.CurrentTag;
            if (tag == null || !tag.IsInputElement())
            {
                return;
            }

            var rules = request.Get<ValidationGraph>().FieldRulesFor(request.Accessor);
            var modifier = request.Get<IFieldValidationModifier>();

            rules.Each(x => modifier.ModifyFor(x, request));
        }
    }
}