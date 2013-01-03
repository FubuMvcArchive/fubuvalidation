using FubuCore.Reflection;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Policies;

namespace FubuMVC.Validation
{
	public class NotValidatedAttributeFilter : IChainFilter
	{
		public bool Matches(BehaviorChain chain)
		{
			var call = chain.FirstCall();
			if (call == null) return true;

			return !call.HasAttribute<NotValidatedAttribute>() && !call.InputType().HasAttribute<NotValidatedAttribute>();
		}
	}
}