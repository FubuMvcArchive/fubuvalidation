using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core.UI.Forms;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation.UI
{
	public class ValidationOptions
	{
		public const string Data = "validation-options";

		private readonly IList<FieldOptions> _fields = new List<FieldOptions>();

		public FieldOptions[] fields { get { return _fields.ToArray(); } }

		protected bool Equals(ValidationOptions other)
		{
			return _fields.SequenceEqual(other._fields);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((ValidationOptions) obj);
		}

		public override int GetHashCode()
		{
			return _fields.GetHashCode();
		}

		public static ValidationOptions For(FormRequest request)
		{
			var services = request.Services;
			var type = services.GetInstance<ITypeResolver>().ResolveType(request.Input);
			var cache = services.GetInstance<ITypeDescriptorCache>();
			var options = new ValidationOptions();
			var node = request.Chain.ValidationNode() as IValidationNode;

			if (node == null)
			{
				return options;
			}

			// TODO -- Let's query the validation graph and register the rule alias/validation mode pairs here
			cache.ForEachProperty(type, property =>
			{
				var accessor = new SingleProperty(property);

				fillFields(options, node, services, accessor);
			});

			return options;
		}

		private static void fillFields(ValidationOptions options, IValidationNode node, IServiceLocator services, Accessor accessor)
		{
			var mode = node.DetermineMode(services, accessor);
			var field = new FieldOptions
			{
				field = accessor.Name,
				mode = mode.Mode
			};

			var graph = services.GetInstance<ValidationGraph>();
			var rules = graph.FieldRulesFor(accessor);
			var ruleOptions = new List<FieldRuleOptions>();

			rules.Each(rule =>
			{
				var ruleMode = rule.Mode ?? mode;
				ruleOptions.Add(new FieldRuleOptions
				{
					rule = RuleAliases.AliasFor(rule),
					mode = ruleMode.Mode
				});
			});

			field.rules = ruleOptions.ToArray();

			options._fields.Add(field);
		}
	}
}