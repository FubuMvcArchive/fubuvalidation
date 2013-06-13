using FubuCore;
using FubuCore.Reflection;
using FubuValidation;

namespace FubuMVC.Validation
{
	public class AccessorRulesValidationModePolicy : IValidationModePolicy
	{
		public bool Matches(IServiceLocator services, Accessor accessor)
		{
			return DetermineMode(services, accessor) != null;
		}

		public ValidationMode DetermineMode(IServiceLocator services, Accessor accessor)
		{
			var rules = services.GetInstance<AccessorRules>();
			return rules.FirstRule<ValidationMode>(accessor);
		}
	}
}