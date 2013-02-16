using System;
using System.Collections.Generic;
using FubuCore;
using FubuCore.Util;
using FubuMVC.Core.UI.Elements;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
	public class LocalizationAnnotationStrategy : IValidationAnnotationStrategy
	{
		public const string LocalizationKey = "localization";

		public bool Matches(IFieldValidationRule rule)
		{
			// TODO -- We might need to make this smarter
			return rule.Token != null && !rule.GetType().Closes(typeof(ConditionalFieldRule<>));
		}

		public void Modify(ElementRequest request, IFieldValidationRule rule)
		{
			var messages = request.CurrentTag.Data(LocalizationKey) as ElementLocalizationMessages;
			if (messages == null)
			{
				messages = new ElementLocalizationMessages();
				request.CurrentTag.Data(LocalizationKey, messages);
			}

			messages.Add(rule);
		}
	}

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