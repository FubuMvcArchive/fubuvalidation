using FubuMVC.Core;
using FubuMVC.Core.Ajax;

namespace FubuMVC.Validation.Remote
{
    public class ValidateField
    {
        public string Hash { get; set; }
        public string Value { get; set; }
    }

    public class ValidateFieldEndpoint
    {
        private readonly RemoteRuleGraph _graph;
        private readonly IRuleRunner _rules;
        private readonly IAjaxContinuationResolver _continuation;

        public ValidateFieldEndpoint(RemoteRuleGraph graph, IRuleRunner rules, IAjaxContinuationResolver continuation)
        {
            _graph = graph;
            _rules = rules;
            _continuation = continuation;
        }

        [UrlPattern("_validation/remote")]
        public AjaxContinuation Validate(ValidateField field)
        {
            var rule = _graph.RuleFor(field.Hash);
            var notification = _rules.Run(rule, field.Value);

            return _continuation.Resolve(notification);
        }
    }
}