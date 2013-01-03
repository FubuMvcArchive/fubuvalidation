using System;

namespace FubuMVC.Validation
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class NotValidatedAttribute : Attribute
	{
	}
}