using FubuMVC.Validation.UI;

namespace FubuMVC.Validation
{
	public interface ValidationNode
	{
		ValidationMode Mode { get; set; }
		RenderingStrategyRegistry Strategies { get; }
	}
}