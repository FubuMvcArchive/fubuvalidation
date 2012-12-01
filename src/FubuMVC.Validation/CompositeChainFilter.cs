using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Policies;

namespace FubuMVC.Validation
{
    // TODO -- This probably goes into FubuMVC.Core
    public class CompositeChainFilter : IChainFilter
    {
        private readonly IList<IChainFilter> _filters = new List<IChainFilter>();

        public CompositeChainFilter(params IChainFilter[] filters)
        {
            _filters.AddRange(filters);
        }

        public bool Matches(BehaviorChain chain)
        {
            return _filters.All(x => x.Matches(chain));
        }
    }
}