using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Validation
{
    public class AjaxValidationNode : Wrapper
    {
        public AjaxValidationNode(ActionCall call)
            : base(typeof(AjaxValidationBehavior<>).MakeGenericType(call.InputType()))
        {
        }
    }
}