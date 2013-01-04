using FubuCore;
using FubuMVC.Core.UI.Forms;
using HtmlTags;

namespace FubuMVC.Validation
{
	public class ValidationMode
	{
		public static readonly ValidationMode None = new ValidationMode("None", false);
		public static readonly ValidationMode LoFi = new ValidationMode("LoFi");
		public static readonly ValidationMode Ajax = new ValidationMode("Ajax");

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

			form.Data("validation-mode", _value.ToLower());
			form.AddClass("validated-form");
		}

		public override string ToString()
		{
			return "Validation Mode: {0}".ToFormat(_value);
		}

		public static ValidationMode For(FormRequest request)
		{
			var validation = request.Chain.ValidationNode();
			return validation == null ? None : validation.Mode;
		}
	}
}