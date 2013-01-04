using FubuCore;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Validation.UI;

namespace FubuMVC.Validation
{
	public class ValidationNode : BehaviorNode
	{
		private readonly BehaviorNode _inner;
		private readonly ValidationMode _mode;
		private readonly RenderingStrategyRegistry _strategies;

		public ValidationNode(BehaviorNode inner, ValidationMode mode)
		{
			_inner = inner;
			_mode = mode;

			_strategies = RenderingStrategyRegistry.Default();
		}

		public BehaviorNode Inner
		{
			get { return _inner; }
		}

		public ValidationMode Mode
		{
			get { return _mode; }
		}

		public RenderingStrategyRegistry Strategies
		{
			get { return _strategies; }
		}

		public override string ToString()
		{
			return "Validation Node: {0}".ToFormat(_mode.Value);
		}

		protected override ObjectDef buildObjectDef()
		{
			return _inner.As<IContainerModel>().ToObjectDef();
		}

		public override BehaviorCategory Category
		{
			get { return BehaviorCategory.Process; }
		}
	}
}