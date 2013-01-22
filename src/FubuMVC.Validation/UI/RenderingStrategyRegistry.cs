using System;
using System.Collections.Generic;

namespace FubuMVC.Validation.UI
{
    // TODO -- Rename this to ValidationNode and kill the original one
    // TODO -- Add the Empty static helper
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
			
            strategies.RegisterStrategy(RenderingStrategies.Summary);
            strategies.RegisterStrategy(RenderingStrategies.Highlight);

            return strategies;
        }
    }
}