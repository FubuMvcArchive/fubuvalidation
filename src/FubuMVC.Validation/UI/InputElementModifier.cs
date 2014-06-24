using System;
using FubuMVC.Core.UI.Elements;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
    public abstract class InputElementModifier : IElementModifier
    {
        public virtual bool Matches(ElementRequest token)
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

            modify(request);
        }


        public void ForRule<T>(ElementRequest request, Action<T> continuation) where T : IFieldValidationRule
        {
            var graph = request.Get<ValidationGraph>();
            graph.Query().ForRule(request.HolderType(), request.Accessor, continuation);
        }

        protected abstract void modify(ElementRequest request);
    }
}