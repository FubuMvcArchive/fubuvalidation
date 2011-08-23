namespace FubuMVC.Validation
{
    public interface IValidationFailureHandler
    {
        void Handle(ValidationFailure context);
    }
}