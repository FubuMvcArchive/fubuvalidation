using System.Collections.Generic;
using System.Linq;
using System.Net;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Policies;

namespace FubuMVC.Validation
{
    public interface IApplyValidationFilter
    {
        bool Filter(BehaviorChain chain);
    }

    public class ValidationSettings : IApplyValidationFilter
    {
        private readonly IList<IChainFilter> _filters = new List<IChainFilter>();

        public ValidationSettings()
        {
            FailAjaxRequestsWith(HttpStatusCode.BadRequest);
        }

        public ChainPredicate Where
        {
            get
            {
                var predicate = new ChainPredicate();
                _filters.Add(predicate);
                
                return predicate;
            }
        }

        public HttpStatusCode StatusCode { get; private set; }

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

        bool IApplyValidationFilter.Filter(BehaviorChain chain)
        {
            return createFilter().Matches(chain);
        }
    }
}