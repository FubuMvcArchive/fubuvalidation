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

    public class ValidationSettings : ValidationSettingsRegistry, IApplyValidationFilter, IChainModification
    {
        private readonly IList<IChainFilter> _filters = new List<IChainFilter>();
        private readonly IList<IRemoteRuleFilter> _remoteFilters = new List<IRemoteRuleFilter>(); 

        public ValidationSettings()
        {
            FailAjaxRequestsWith(HttpStatusCode.BadRequest);
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

		private IChainFilter filter
		{
			get
			{
				if (_filters.Any())
				{
					return new CompositeChainFilter(_filters.ToArray());
				}

				return new DefaultValidationChainFilter();
			}
		}

		public void Import<T>()
			where T : ValidationSettingsRegistry, new()
		{
			var registry = new T();
			registry.Modifications.Each(addModification);
		}

		public void FailAjaxRequestsWith(HttpStatusCode statusCode)
		{
			StatusCode = statusCode;
		}

        bool IApplyValidationFilter.Filter(BehaviorChain chain)
        {
            return filter.Matches(chain);
        }

	    void IChainModification.Modify(BehaviorChain chain)
	    {
			Modifications
				.Where(x => x.Matches(chain))
				.Each(x => x.Modify(chain));
	    }
    }
}