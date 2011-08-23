namespace FubuMVC.Validation
{
    public interface IValidationFailurePolicy
    {
        bool Matches(ValidationFailure context);
        void Handle(ValidationFailure context);
    }
}