using System;
using System.Collections.Generic;
using FubuMVC.Core.UI.Forms;

namespace FubuMVC.Validation.UI
{
	public interface IRenderingStrategy
	{
		void Modify(FormRequest request);
	}

	public class RenderingStrategy : IRenderingStrategy
	{
		public static readonly RenderingStrategy Summary = new RenderingStrategy("Summary", x => x.CurrentTag.Data("validation-summary", true));
		public static readonly RenderingStrategy Highlight = new RenderingStrategy("Highlight", x => x.CurrentTag.Data("validation-highlight", true));

		private readonly string _name;
		private readonly Action<FormRequest> _modify;

		public RenderingStrategy(string name, Action<FormRequest> modify)
		{
			_name = name;
			_modify = modify;
		}

		public string Name { get { return _name; } }

		public void Modify(FormRequest request)
		{
			_modify(request);
		}
	}

	public class RenderingStrategyRegistry
	{
		private readonly IList<IRenderingStrategy> _strategies = new List<IRenderingStrategy>();

		public void RegisterStrategy(IRenderingStrategy strategy)
		{
			_strategies.Fill(strategy);
		}

		public IEnumerable<IRenderingStrategy> All()
		{
			return _strategies;
		}

		public void Each(Action<IRenderingStrategy> action)
		{
			_strategies.Each(action);
		}

		public void Clear()
		{
			_strategies.Clear();
		}

		public static RenderingStrategyRegistry Default()
		{
			var strategies = new RenderingStrategyRegistry();
			
			strategies.RegisterStrategy(RenderingStrategy.Summary);
			strategies.RegisterStrategy(RenderingStrategy.Highlight);

			return strategies;
		}
	}
}