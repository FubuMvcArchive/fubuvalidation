using System;
using System.Collections.Generic;
using FubuCore.Util;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
    public class ElementLocalizationMessages
	{
		private static readonly Cache<Type, string> Aliases = new Cache<Type, string>(createRuleAlias);
		private readonly IDictionary<string, string> _messages = new Dictionary<string, string>();

		private static string createRuleAlias(Type type)
		{
			// RequiredFieldRule => required
			return type.Name.Replace("Field", "").Replace("Rule", "").ToLower();
		}

		public IDictionary<string, string> Messages { get { return _messages; } }

		public void Add(IFieldValidationRule rule)
		{
			var key = Aliases[rule.GetType()];
			_messages.Fill(key, rule.Token.ToString());
		}
	}
}