using FubuCore;
using FubuCore.Reflection;
using FubuValidation;

namespace FubuMVC.Validation
{
	public interface IValidationModePolicy
	{
		bool Matches(IServiceLocator services, Accessor accessor);
		ValidationMode DetermineMode(IServiceLocator services, Accessor accessor);
	}
}