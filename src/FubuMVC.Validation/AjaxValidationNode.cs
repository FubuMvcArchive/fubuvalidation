using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Validation.UI;

namespace FubuMVC.Validation
{
    public class AjaxValidationNode : Wrapper, IHaveValidation
    {
        public AjaxValidationNode(ActionCall call)
            : base(typeof(AjaxValidationBehavior<>).MakeGenericType(call.InputType()))
        {
        	Validation = ValidationNode.DefaultFor(ValidationMode.Ajax);
        }

        public ValidationNode Validation { get; private set; }
    }
}