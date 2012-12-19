using FubuValidation;

namespace FubuMVC.Validation.Remote
{
    public interface IRuleRunner
    {
        Notification Run(RemoteFieldRule rule, string value);
    }

    public class RuleRunner : IRuleRunner
    {
        public Notification Run(RemoteFieldRule rule, string value)
        {
            throw new System.NotImplementedException();
        }
    }
}