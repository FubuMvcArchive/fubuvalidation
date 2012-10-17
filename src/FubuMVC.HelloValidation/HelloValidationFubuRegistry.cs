using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.HelloValidation.Handlers;
using FubuMVC.HelloValidation.Handlers.Products;
using FubuMVC.Validation;

namespace FubuMVC.HelloValidation
{
    public class HelloValidationFubuRegistry : FubuRegistry
    {
        public HelloValidationFubuRegistry()
        {
            Import<HandlerConvention>(x => x.MarkerType<HandlersMarker>());

            Routes
                .HomeIs<GetHandler>(h => h.Execute(new ProductsListRequestModel()));

            this.Validation(validation =>
                                {
                                    validation
                                        .Actions
                                        .Include(call => call.IsHttpPost());

                                    // This DSL reads as follows...
                                    // When handling failures:
                                    //  If the input type of the action call is not null and the name of the model contains the string "Input",
                                    //  Then Transfer to a behavior chain that is resolved by my custom HandlerModelDescriptor class
                                    validation
                                        .Failures
                                        .If(f => f.InputType() != null && f.InputType().Name.Contains("Input"))
                                        .TransferBy<HandlerModelDescriptor>();

                                    // The above call makes use of the default FubuRequestInputModelResolver class.
                                    // This class is used to build up the appropriate model (from the FubuRequest). 
                                    // In the event that you have complex binding, you may want to do an object mapping call here
                                    // Either by hand (that is, specific to your types) or via AutoMapper or something
                                });
        }
    }

    public static class FubuSemanticExtensions
    {
        public static bool IsHttpPost(this BehaviorChain chain)
        {
            var route = chain.Route;
            if (route == null)
            {
                return false;
            }

            return route.AllowedHttpMethods.Any(c => c.ToLower() == "post");
        }

        public static bool IsHttpPost(this ActionCall call)
        {
            return call.ParentChain().IsHttpPost();
        }
    }
}