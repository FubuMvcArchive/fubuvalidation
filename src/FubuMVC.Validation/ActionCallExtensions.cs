using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Validation
{
    public static class ActionCallExtensions
    {
        public static void WrapWithValidation(this ActionCall call)
        {
            call.AddBefore(new ValidationNode(call));
        }
    }
}