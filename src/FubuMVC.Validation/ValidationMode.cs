using System.Linq;
using FubuCore;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.UI.Forms;
using HtmlTags;

namespace FubuMVC.Validation
{
	public class ValidationMode
	{
		public static readonly ValidationMode None = new ValidationMode("None", false);
		public static readonly ValidationMode LoFi = new ValidationMode("lofi");
		public static readonly ValidationMode Ajax = new ValidationMode("ajax");

		private readonly string _value;
		private readonly bool _modify;

		public ValidationMode(string value, bool modify = true)
		{
			_value = value;
			_modify = modify;
		}

		public string Value { get { return _value; } }

		public void Modify(HtmlTag form)
		{
			if (!_modify) return;

			form.Data("validation-mode", _value);
			form.AddClass("validated-form");
		}

		public static ValidationMode For(FormRequest request)
		{
			var chain = request.Chain;
			if(chain.OfType<AjaxValidationNode>().Any())
			{
				return Ajax;
			}

			if(chain.OfType<ActionFilter>().Any(x => x.HandlerType.Closes(typeof(ValidationActionFilter<>))))
			{
				return LoFi;
			}

			return None;
		}
	}
}