using System.Collections.Generic;
using System.Linq;
using System.Net;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Policies;
using FubuMVC.Validation.Remote;

namespace FubuMVC.Validation
{
    public interface IApplyValidationFilter
    {
        bool Filter(BehaviorChain chain);
    }

    public class ValidationSettings : IApplyValidationFilter
    {
        private readonly IList<IChainFilter> _filters = new List<IChainFilter>();
        private readonly IList<IRemoteRuleFilter> _remoteFilters = new List<IRemoteRuleFilter>(); 

        public ValidationSettings()
        {
            FailAjaxRequestsWith(HttpStatusCode.BadRequest);
        }

        public void FailAjaxRequestsWith(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        private IChainFilter createFilter()
        {
            if(_filters.Any())
            {
                return new CompositeChainFilter(_filters.ToArray());
            }

            return new DefaultValidationChainFilter();
        }

        public HttpStatusCode StatusCode { get; private set; }
        public RemoteRuleExpression Remotes { get{ return new RemoteRuleExpression(_remoteFilters);}}
        public IEnumerable<IRemoteRuleFilter> Filters { get { return _remoteFilters; } } 

        public ChainPredicate Where
        {
            get
            {
                var predicate = new ChainPredicate();
                _filters.Add(predicate);

                return predicate;
            }
        }

        bool IApplyValidationFilter.Filter(BehaviorChain chain)
        {
            return createFilter().Matches(chain);
        }
    }
}