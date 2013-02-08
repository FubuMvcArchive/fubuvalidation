using System;
using FubuCore.Descriptions;
using FubuMVC.Core;
using FubuValidation;

namespace FubuMVC.Validation.Diagnostics
{
	[FubuPartial]
	public class ValidationDiagnostics
	{
		private readonly ValidationGraph _graph;

		public ValidationDiagnostics(ValidationGraph graph)
		{
			_graph = graph;
		}

		public Description Ajax(AjaxValidationNode node)
		{
			return visualize(node.InputType);
		}

		public Description LoFi(ValidationActionFilter filter)
		{
			return visualize(filter.InputType());
		}

		private Description visualize(Type inputType)
		{
			var plan = _graph.PlanFor(inputType);
			return Description.For(plan);
		}
	}
}