using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;

namespace FubuMVC.Validation
{
    public class ValidationNode : Wrapper
    {
        private readonly ActionCall _call;

        public ValidationNode(ActionCall call)
            : base(typeof(ValidationBehavior<>).MakeGenericType(call.InputType()))
        {
            _call = call;
        }

        protected override ObjectDef buildObjectDef()
        {
            var def = base.buildObjectDef();
            def.DependencyByValue(_call);
            return def;
        }
    }
}