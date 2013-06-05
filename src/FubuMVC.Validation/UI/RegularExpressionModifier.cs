using FubuMVC.Core.UI.Elements;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
	public class RegularExpressionModifier : InputElementModifier
	{
		protected override void modify(ElementRequest request)
		{
			ForRule<RegularExpressionFieldRule>(request, rule => request.CurrentTag.Data("regex", rule.Expression.ToString()));
		}
	}
}