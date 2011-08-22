namespace FubuMVC.Validation
{
    public interface IValidationFailureHandler
    {
        void Handle(ValidationFailureContext context);
    }
}