using System.Linq;
using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Validation
{
	public static class ValidationBehaviorChainExtensions
	{
		 public static ValidationNode ValidationNode(this BehaviorChain chain)
		 {
			 return chain.OfType<ValidationNode>().SingleOrDefault();
		 }
	}
}