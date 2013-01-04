using FubuMVC.Validation.UI;

namespace FubuMVC.Validation
{
	public interface ValidationNode
	{
		ValidationMode Mode { get; }
		RenderingStrategyRegistry Strategies { get; }
	}
}