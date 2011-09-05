using FluentValidation;
using FubuMVC.HelloValidation.Handlers.Products.Create;

namespace FubuMVC.HelloValidation
{
    public class FluentProductValidator : AbstractValidator<CreateProductInputModel>
    {
        public FluentProductValidator()
        {
            RuleFor(m => m.Price)
                .GreaterThan(0);
        }
    }
}