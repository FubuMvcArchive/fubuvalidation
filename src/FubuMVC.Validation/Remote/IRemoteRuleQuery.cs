using System.Collections.Generic;
using System.Linq;
using FubuValidation.Fields;

namespace FubuMVC.Validation.Remote
{
    public interface IRemoteRuleQuery
    {
        bool IsRemote(IFieldValidationRule rule);
    }

    public class RemoteRuleQuery : IRemoteRuleQuery
    {
        private readonly IEnumerable<IRemoteRuleFilter> _filters;

        public RemoteRuleQuery(IEnumerable<IRemoteRuleFilter> filters)
        {
            _filters = filters;
        }

        public bool IsRemote(IFieldValidationRule rule)
        {
            return _filters.Any(x => x.Matches(rule));
        }

        public static RemoteRuleQuery Basic()
        {
            return new RemoteRuleQuery(new IRemoteRuleFilter[] { new RemoteRuleAttributeFilter(), new RemoteFieldValidationRuleFilter() });
        }
    }
}