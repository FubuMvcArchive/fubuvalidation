using FubuMVC.Conventions;
using FubuMVC.Core;
using FubuMVC.HelloValidation.Handlers;
using FubuMVC.HelloValidation.Handlers.Products;
using FubuMVC.Spark;
using FubuMVC.Validation;
using FubuValidation;

namespace FubuMVC.HelloValidation
{
    public class HelloValidationFubuRegistry : FubuRegistry
    {
        public HelloValidationFubuRegistry()
        {
            IncludeDiagnostics(true);

            this
                .ApplyHandlerConventions<HandlersMarker>();

            Routes
                .HomeIs<GetHandler>(h => h.Execute(new ProductsListRequestModel()));

            this.UseSpark();
            
            Views
                .TryToAttachWithDefaultConventions()
                .RegisterActionLessViews(t => t.ViewModelType == typeof(Notification));

            this.Validation(validation =>
                                {
                                    // Include all action calls that: 1) have input and 2) whose input models contain the string "Input"
                                    // We use a convention in this sample that models for POST ActionCalls contain "Input"
                                    validation
                                        .Actions
                                        .Include(call => call.HasInput && call.InputType().Name.Contains("Input"));

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
}