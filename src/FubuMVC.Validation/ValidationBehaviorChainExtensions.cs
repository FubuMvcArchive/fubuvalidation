using System.Linq;
using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Validation
{
	public static class ValidationBehaviorChainExtensions
	{
		 public static ValidationNode ValidationNode(this BehaviorChain chain)
		 {
			 var node = chain.OfType<IHaveValidation>().SingleOrDefault();
             if (node != null)
             {
                 return node.Validation;
             }

		     return Validation.ValidationNode.Empty();
		 }
	}
}