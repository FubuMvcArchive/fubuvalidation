using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Policies;

namespace FubuMVC.Validation
{
    // TODO -- Pull this into FubuMVC.Core
    public class HasInputType : IChainFilter
    {
        public bool Matches(BehaviorChain chain)
        {
            return chain.InputType() != null;
        }
    }
}