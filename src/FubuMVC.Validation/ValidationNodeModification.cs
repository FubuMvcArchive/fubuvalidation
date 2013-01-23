using System;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Policies;

namespace FubuMVC.Validation
{
	public class ValidationNodeModification
	{
		private readonly IChainFilter _filter;
		private readonly Action<ValidationNode> _action;

		public ValidationNodeModification(IChainFilter filter, Action<ValidationNode> action)
		{
			_filter = filter;
			_action = action;
		}

		public bool Matches(BehaviorChain chain)
		{
			return _filter.Matches(chain);
		}

		public void Modify(BehaviorChain chain)
		{
			_action(chain.ValidationNode());
		}
	}
}