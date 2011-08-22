namespace FubuMVC.Validation
{
    public interface IValidationFailurePolicy
    {
        bool Matches(ValidationFailureContext context);
        void Handle(ValidationFailureContext context);
    }
}