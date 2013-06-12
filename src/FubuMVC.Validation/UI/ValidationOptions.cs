using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core.UI.Forms;

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
			var type = request.Services.GetInstance<ITypeResolver>().ResolveType(request.Input);
			var cache = request.Services.GetInstance<ITypeDescriptorCache>();
			var options = new ValidationOptions();
			var node = request.Chain.ValidationNode() as IValidationNode;

			if (node == null)
			{
				return options;
			}

			cache.ForEachProperty(type, property =>
			{
				var mode = node.DetermineMode(request.Services, new SingleProperty(property));
				options._fields.Add(new FieldOptions
				{
					field = property.Name,
					mode = mode.Mode
				});
			});

			return options;
		}
	}

	public class FieldOptions
	{
		public string field { get; set; }
		public string mode { get; set; }

		public override string ToString()
		{
			return "{0} ({1})".ToFormat(field, mode);
		}

		protected bool Equals(FieldOptions other)
		{
			return string.Equals(field, other.field) && string.Equals(mode, other.mode);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((FieldOptions) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (field.GetHashCode()*397) ^ mode.GetHashCode();
			}
		}
	}
}