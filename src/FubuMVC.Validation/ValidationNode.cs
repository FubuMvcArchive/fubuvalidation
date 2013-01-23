using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.UI.Forms;
using FubuMVC.Validation.UI;

namespace FubuMVC.Validation
{
    public class ValidationNode : IEnumerable<IRenderingStrategy>
    {
        private readonly IList<IRenderingStrategy> _strategies = new List<IRenderingStrategy>();

        public ValidationNode()
            : this(ValidationMode.None)
        {
        }

        public ValidationNode(ValidationMode mode)
        {
            Mode = mode;
        }

        public ValidationMode Mode { get; set; }

		public void Modify(FormRequest request)
		{
			Mode.Modify(request.CurrentTag);
			Each(x => x.Modify(request));
		}

        public void RegisterStrategy(IRenderingStrategy strategy)
        {
            _strategies.Fill(strategy);
        }

        public void Each(Action<IRenderingStrategy> action)
        {
            _strategies.Each(action);
        }

        public void Clear()
        {
            _strategies.Clear();
        }

	    public IEnumerator<IRenderingStrategy> GetEnumerator()
	    {
		    return _strategies.GetEnumerator();
	    }

	    public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ValidationNode) obj);
        }

        protected bool Equals(ValidationNode other)
        {
            return _strategies.SequenceEqual(other._strategies) && Mode.Equals(other.Mode);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_strategies.GetHashCode() * 397) ^ Mode.GetHashCode();
            }
        }

	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return GetEnumerator();
	    }

	    public static ValidationNode Empty()
        {
            return new ValidationNode();
        }

        public static ValidationNode DefaultFor(ValidationMode mode)
        {
            var validation = new ValidationNode(mode);

            validation.RegisterStrategy(RenderingStrategies.Summary);
            validation.RegisterStrategy(RenderingStrategies.Highlight);

            return validation;
        }
    }
}